using System.Text.Json.Serialization;

namespace TrainManager_Suelen_Vicente
{
    public class Seat
    {
        public int row { get; private set; }
        public int column { get; private set; }

        public Passenger passenger { get; private set; }

        [JsonConstructor]
        public Seat(int row, int column, Passenger passenger)
        {
            this.row = row;
            this.column = column;
            this.passenger = passenger;
        }

        public Seat(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public void addPassenger(Passenger passenger)
        {
            this.passenger = passenger;
        }

        public void removePassenger()
        {
            this.passenger = null;
        }

        public string getSeatName()
        {
            return this.row.ToString() + Utils.convertNumberIntoAlphabet(this.column);
        }
    }
}
