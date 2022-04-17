using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace TrainManager_Suelen_Vicente
{
    /* Suelen Cristina Blasques Goes Vicente
     * 8752253
     * PROG8145 W22
     * Conestoga College
     */

    public enum OperationMode
    {
        ManageSeats,
        Search
    }

    public enum States
    {
        Initial,
        SearchEmpty,
        SelectedSeatOccupied,
        SelectedSeatAvailable,
        ClearSearch
    }
    
    public partial class MainWindow : Window
    {
        private Seat selectedSeat;
        private Train train;
        private FileManager fileManager;
        private bool isAppStarting = false;

        public MainWindow()
        {
            InitializeComponent();

            train = new Train();
            fileManager = new FileManager();
            //train.fillSomeSeats();

            isAppStarting = true;

            loadFileComboBox();

            cmbFiles.SelectedIndex = 0;

            if (!fileManager.currentFileIsEmpty())
                train.loadSeatChart(fileManager.getCurrentFile());

            configState(States.Initial);

            isAppStarting = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            configState(States.Initial);
        }

        private void loadFileComboBox()
        {
            fileManager.loadAllJsonFiles();

            btnSave.IsEnabled = fileManager.existFilesToLoad();

            updateFileComboBox();
        }

        private void updateFileComboBox()
        {
            cmbFiles.Items.Clear();
            foreach (var fileName in fileManager.getAllFiles())
                cmbFiles.Items.Add(fileName);

            if (!fileManager.currentFileIsEmpty())
                cmbFiles.SelectedItem = fileManager.getCurrentFile();
        }

        private void cmbFiles_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(!isAppStarting)
                fileManager.saveFile(train.convertSeatChartToJSON());

            if (cmbFiles.Items.Count > 0)
            {
                fileManager.setCurrentFile(cmbFiles.SelectedItem.ToString());
                train.loadSeatChart(fileManager.getCurrentFile());
                configState(States.Initial);
            }
        }

        private void btnBtnSeat_Click(object sender, RoutedEventArgs e)
        {
            Button btnSeat = sender as Button;

            if (btnSeat != null)
                selectSeat(btnSeat);
        }

        private void btnAddPassenger_Click(object sender, RoutedEventArgs e)
        {
            if(txtPassengerFullName.Text == null || txtPassengerFullName.Text == "")
            {
                MessageBox.Show("You must inform a passenger name to continue.");
                txtPassengerFullName.Focus();
            }
            else
            {
                Passenger passenger = new Passenger(txtPassengerFullName.Text);

                selectedSeat.addPassenger(passenger);

                MessageBox.Show("Passenger "+ passenger.fullName +" added to seat " + selectedSeat.getSeatName() + ".");
                if (train.isFull())
                {
                    MessageBox.Show("The train is full!");
                    configState(States.Initial);
                }
                else
                {
                    configState(States.SelectedSeatOccupied);
                }
            }
        }

        private void btnRemovePassenger_Click(object sender, RoutedEventArgs e)
        {
            Passenger passenger = new Passenger(txtPassengerFullName.Text);

            if (txtSelectedSeat.Text == null || txtSelectedSeat.Text == "")
                selectedSeat = train.searchSeatByPassenger(passenger);

            selectedSeat.removePassenger();

            MessageBox.Show("Passenger removed.");

            configState(States.SelectedSeatAvailable);
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            train.deleteAll();
            configState(States.Initial);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            configState(States.ClearSearch);
            txtSearchPassengerFullName.Focus();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            selectedSeat = train.returnSeatByPositionOrPassenger(txtSearchPassengerFullName.Text, txtSearchSeat.Text);

            if (selectedSeat == null)
            {
                configState(States.SearchEmpty);
            }
            else
            {
                var btnSeat = getBtnSeatSelectedByGridPosition();
                selectSeat(btnSeat);

                var state = selectedSeat.passenger == null ? States.SelectedSeatAvailable : States.SelectedSeatOccupied;
                configState(state);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(fileManager.saveFile(train.convertSeatChartToJSON()));
        }

        private void btnSaveNew_Click(object sender, RoutedEventArgs e)
        {
            saveNewFile();

        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var json = File.ReadAllText(openFileDialog.FileName);
                train.loadSeatChart(json);
                paintAllSeats();
            }
        }

        private void btnHowToUse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Utils.howToUse());
        }

        private void saveNewFile()
        {
            SaveNewFile saveNewFileWindow = new SaveNewFile(train.convertSeatChartToJSON(), fileManager);
            saveNewFileWindow.ShowDialog();

            fileManager = saveNewFileWindow.fileManager;

            loadFileComboBox();
        }

        private Button getBtnSeatSelectedByGridPosition()
        {
            foreach (UIElement element in gridSeatChart.Children)
            {
                if (Grid.GetColumn(element) == selectedSeat.column && Grid.GetRow(element) == selectedSeat.row)
                    return (Button)element;
            }

            return null;
        }

        private void paintAllSeats()
        {
            foreach (UIElement element in gridSeatChart.Children)
            {
                if (Grid.GetColumn(element) < Train.numberOfColumns && Grid.GetRow(element) > 0)
                {
                    Button btnSeat = (Button)element;
                    var row = Grid.GetRow(btnSeat);
                    var col = Grid.GetColumn(btnSeat);
                    Seat seat = train.searchSeatByPosition(row, col);
                    var seatName = seat.getSeatName();

                    if (seat.passenger == null)
                    {
                        btnSeat.Content = seatName;
                        btnSeat.Background = Brushes.SpringGreen;
                    }
                    else
                    {
                        btnSeat.Content = "[" + seatName + "]";
                        btnSeat.Background = Brushes.IndianRed;
                    }
                }
            }
        }

        private void highlightSeat(Button btnSeat)
        {
            paintAllSeats();
            btnSeat.Background = Brushes.SkyBlue;
        }

        private void selectSeat(Button btnSeat)
        {
            highlightSeat(btnSeat);

            var row = Grid.GetRow(btnSeat);
            var col = Grid.GetColumn(btnSeat);
            selectedSeat = train.searchSeatByPosition(row, col);
            var seatNumberConverted = selectedSeat.getSeatName();

            var state = selectedSeat.passenger == null ? States.SelectedSeatAvailable : States.SelectedSeatOccupied;
            configState(state);
            txtSelectedSeat.Text = seatNumberConverted;
            txtSearchedSeat.Text = seatNumberConverted;
            btnSeat.Content = "*" + seatNumberConverted;
        }

        private void configState(States state)
        {
            switch (state)
            {
                case States.Initial:
                    paintAllSeats();
                    clearAll();
                    //Manage Seats Tab
                    txtPassengerFullName.IsEnabled = false;
                    btnAddPassenger.IsEnabled = false;
                    btnRemovePassenger.IsEnabled = false;
                    lblSeatStatus.Content = "";
                    txtSelectedSeat.IsEnabled = false;
                    txtSelectedPassenger.IsEnabled = false;
                    //Search Tab
                    btnRemovePassengerSearch.IsEnabled = false;
                    btnSearch.IsEnabled = true;
                    btnClear.IsEnabled = false;
                    txtSearchPassengerFullName.IsEnabled = true;
                    txtSearchSeat.IsEnabled = true;
                    txtSearchedPassenger.Text = "";
                    txtSearchedSeat.Text = "";
                    txtSearchPassengerFullName.Text = "";
                    txtSearchSeat.Text = "";
                    break;
                case States.SelectedSeatOccupied:
                    //Manage Seats Tab
                    lblSeatStatus.Content = "This seat is occupied.";
                    txtSelectedPassenger.Text = selectedSeat.passenger.fullName;
                    txtPassengerFullName.Text = "";
                    txtPassengerFullName.IsEnabled = false;
                    btnAddPassenger.IsEnabled = false;
                    btnRemovePassenger.IsEnabled = true;
                    //Search Tab
                    lblSearchStatus.Content = "Passenger/Seat found.";
                    lblSearchStatus.Content += " This seat is occupied.";
                    txtSearchedPassenger.Text = selectedSeat.passenger.fullName;
                    txtSearchedSeat.Text = selectedSeat.getSeatName();
                    txtSearchPassengerFullName.Text = "";
                    txtSearchSeat.Text = "";
                    txtSearchSeat.IsEnabled = false;
                    txtSearchPassengerFullName.IsEnabled = false;
                    btnRemovePassengerSearch.IsEnabled = true;
                    btnSearch.IsEnabled = false;
                    btnClear.IsEnabled = true;
                    break;
                case States.SelectedSeatAvailable:
                    //Manage Seats Tab
                    lblSeatStatus.Content = "This seat is available";
                    txtSelectedPassenger.Text = "";
                    txtPassengerFullName.Text = "";
                    txtPassengerFullName.IsEnabled = true;
                    btnAddPassenger.IsEnabled = true;
                    btnRemovePassenger.IsEnabled = false;
                    // Search Tab
                    lblSearchStatus.Content = "Passenger/Seat found.";
                    lblSearchStatus.Content += " This seat is available.";
                    txtSearchedPassenger.Text = "";
                    txtSearchedSeat.Text = selectedSeat.getSeatName();
                    txtSearchPassengerFullName.Text = "";
                    txtSearchSeat.Text = "";
                    txtSearchSeat.IsEnabled = false;
                    txtSearchPassengerFullName.IsEnabled = false;
                    btnRemovePassengerSearch.IsEnabled = false;
                    btnSearch.IsEnabled = false;
                    btnClear.IsEnabled = true;
                    break;
                case States.SearchEmpty:
                    lblSearchStatus.Content = "Zero results found. Search Again.";
                    txtSearchedSeat.Text = "";
                    txtSearchedPassenger.Text = "";
                    txtSearchPassengerFullName.Text = "";
                    txtSearchSeat.Text = "";
                    txtSearchSeat.IsEnabled = false;
                    txtSearchPassengerFullName.IsEnabled = false;
                    btnSearch.IsEnabled = false;
                    btnClear.IsEnabled = true;
                    break;
                case States.ClearSearch:
                    paintAllSeats();
                    clearSearchFields();
                    btnSearch.IsEnabled = true;
                    btnClear.IsEnabled = false;
                    txtSearchSeat.IsEnabled = true;
                    txtSearchPassengerFullName.IsEnabled = true;
                    break;
            }
        }

        private void clearAll()
        {
            clearManageSeatsFields();
            clearSearchFields();
        }

        private void clearManageSeatsFields()
        {
            txtPassengerFullName.Text = "";
            txtSelectedSeat.Text = "";
            lblSeatStatus.Content = "";
            txtSelectedPassenger.Text = "";
        }

        private void clearSearchFields()
        {
            txtSearchPassengerFullName.Text = "";
            txtSearchSeat.Text = "";
            txtSearchedPassenger.Text = "";
            txtSearchedSeat.Text = "";
            lblSearchStatus.Content = "";
        }
    }
}
