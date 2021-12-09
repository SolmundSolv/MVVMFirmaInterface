using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    //to jest 
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        //to jest 
        private ReadOnlyCollection<CommandViewModel> _Commands;
        //to jest 
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands

        private BaseCommand _NowyTowarCommand;
        public ICommand NowyTowarCommand
        {
            get
            {
                if (_NowyTowarCommand == null)
                    _NowyTowarCommand = new BaseCommand(() => CreateView(new NowyTowarViewModel()));
                return _NowyTowarCommand;
            }
        }
        private BaseCommand _MagazynCommand;
        public ICommand MagazynCommand
        {
            get
            {
                if (_MagazynCommand == null)
                    _MagazynCommand = new BaseCommand(() => CreateView(new MagazynyViewModel()));
                return _MagazynCommand;
            }
        }   
        
        private BaseCommand _MapaCommand;
        public ICommand MapaCommand
        {
            get
            {
                if (_MapaCommand == null)
                    _MapaCommand = new BaseCommand(() => CreateView(new MapaViewModel()));
                return _MapaCommand;
            }
        }

        private BaseCommand _NowaFakturaCommand;
        public ICommand NowaFakturaCommand
        {
            get
            {
                if (_NowaFakturaCommand == null)
                    _NowaFakturaCommand = new BaseCommand(() => CreateView(new NowaFakturaViewModel()));
                return _NowaFakturaCommand;
            }
        }
        
        private BaseCommand _NowyPracownikCommand;
        public ICommand NowyPracownikCommand
        {
            get
            {
                if (_NowyPracownikCommand == null)
                    _NowyPracownikCommand = new BaseCommand(() => CreateView(new NowyPracownikViewModel()));
                return _NowyPracownikCommand;
            }
        }
        
        private BaseCommand _RaportyCommand;
        public ICommand RaportyCommand
        {
            get
            {
                if (_RaportyCommand == null)
                    _RaportyCommand = new BaseCommand(() => CreateView(new RaportyViewModel()));
                return _RaportyCommand;
            }
        }
        
        private BaseCommand _BazaDanychCommand;
        public ICommand BazaDanychCommand
        {
            get
            {
                if (_BazaDanychCommand == null)
                    _BazaDanychCommand = new BaseCommand(() => CreateView(new BazaDanychViewModel()));
                return _BazaDanychCommand;
            }
        }


        private BaseCommand _WszystkieTowaryCommand;
        public ICommand WszystkieTowaryCommand
        {
            get
            {
                if (_WszystkieTowaryCommand == null)
                    _WszystkieTowaryCommand = new BaseCommand(ShowAllTowar);
                return _WszystkieTowaryCommand;
            }
        }
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }

        private List<CommandViewModel> CreateCommands()
        {

            return new List<CommandViewModel>
            {
                //tu
                new CommandViewModel(
                    "Towary",
                    new BaseCommand(() => this.ShowAllTowar())),

                new CommandViewModel(
                    "Towar",
                    new BaseCommand(() => this.CreateTowar())),
                new CommandViewModel(
                    "Magazyny",
                    new BaseCommand (()=> this.ShowMagazyn())),
                new CommandViewModel(
                    "Faktura",
                    new BaseCommand (()=> this.CreateFaktura())),
                new CommandViewModel(
                    "Raporty",
                    new BaseCommand(() => this.ShowAllRaporty())),
                new CommandViewModel(
                    "Nowy Pracownik",
                    new BaseCommand(() => this.CreatePracownik())),
                new CommandViewModel(
                    "Mapa",
                    new BaseCommand(() => this.ShowMapy())),
                new CommandViewModel(
                    "Baza danych",
                    new BaseCommand(() => this.ShowBazaDanych()))

            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers
        //
        //
        private void CreateView(WorkspaceViewModel workspace)
        {
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        private void CreateTowar()
        {
            NowyTowarViewModel workspace = new NowyTowarViewModel();
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        private void CreateFaktura()
        {
            NowaFakturaViewModel workspace = new NowaFakturaViewModel();
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        private void CreatePracownik()
        {
            NowyPracownikViewModel workspace = new NowyPracownikViewModel();
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        //
        private void ShowAllTowar()
        {
            WszystkieTowaryViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is WszystkieTowaryViewModel)
                as WszystkieTowaryViewModel;
            if (workspace == null)
            {
                workspace = new WszystkieTowaryViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }private void ShowBazaDanych()
        {
            BazaDanychViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is BazaDanychViewModel)
                as BazaDanychViewModel;
            if (workspace == null)
            {
                workspace = new BazaDanychViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        private void ShowMagazyn()
        {
            MagazynyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is MagazynyViewModel)
                as MagazynyViewModel;
            if (workspace == null)
            {
                workspace = new MagazynyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        private void ShowMapy()
        {
            MapaViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is MapaViewModel)
                as MapaViewModel;
            if (workspace == null)
            {
                workspace = new MapaViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
      
        private void ShowAllRaporty()
        {
            RaportyViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is RaportyViewModel)
                as RaportyViewModel;
            if (workspace == null)
            {
                workspace = new RaportyViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        //
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}
