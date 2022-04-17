using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace TrainManager_Suelen_Vicente
{
    public class Train
    {
        public static int numberOfRows = 6;
        public static int numberOfColumns = 4;

        public List<Seat> seatChart;

        public Train()
        {
            seatChart = new List<Seat>();
            for (int row = 1; row <= numberOfRows; row++)
            {
                for(int col = 0; col < numberOfColumns; col++)
                {
                    seatChart.Add(new Seat(row, col));
                }
            }
        }

        public void fillSomeSeats()
        {
            var link = new Passenger("Link of Hyrule");
            var zelda = new Passenger("Zelda of Hyrule");
            var mario = new Passenger("Mario Plumber");
            var luiggi = new Passenger("Luiggi Plumber");
            var peach = new Passenger("Peach Princess");
            var donkey = new Passenger("Donkey Kong");

            var seat1 = searchSeatByPosition(1, 1);
            seat1.addPassenger(peach);

            var seat2 = searchSeatByPosition(2, 2);
            seat2.addPassenger(zelda);

            var seat3 = searchSeatByPosition(2, 3);
            seat3.addPassenger(link);

            var seat4 = searchSeatByPosition(3, 1);
            seat4.addPassenger(luiggi);

            var seat5 = searchSeatByPosition(6, 2);
            seat5.addPassenger(mario);

            var seat6 = searchSeatByPosition(5, 0);
            seat6.addPassenger(donkey);
        }

        public Seat searchSeatByPassenger(Passenger passenger)
        {
            return seatChart.FirstOrDefault(x => x.passenger != null && x.passenger.fullName == passenger.fullName);
        }

        public Seat searchSeatByPosition(int row, int column)
        {
            return seatChart.FirstOrDefault(x => x.row == row && x.column == column);
        }

        public Passenger searchPassengerBySeat(Seat seat)
        {
            Seat filteredSeat = searchSeatByPosition(seat.row, seat.column);
            return filteredSeat.passenger;
        }

        public List<Seat> searchAvailablesSeats()
        {
            return seatChart.Where(x => x.passenger == null).ToList();
        }

        public List<Seat> searchOccupiedSeats()
        {
            return seatChart.Where(x => x.passenger != null).ToList();
        }

        public Seat returnSeatByPositionOrPassenger(string passengerFullName, string seatPosition)
        {
            if (seatPosition != null && seatPosition != "")
            {
                var row = seatPosition[0] - '0';
                var col = Utils.convertLetterIntoNumber(seatPosition[1]);

                return searchSeatByPosition(row, col - 1);
            }
            else if (passengerFullName != null && passengerFullName != "")
            {
                var passenger = new Passenger(passengerFullName);
                return searchSeatByPassenger(passenger);
            }

            return null;
        }

        public int countAvailableSeats()
        {
            List<Seat> availableSeats = searchAvailablesSeats();
            return availableSeats.Count();
        }

        public void deleteAll()
        {
            foreach (var seat in seatChart)
                seat.removePassenger();
        }

        public bool isFull()
        {
            List<Seat> availableSeats = searchAvailablesSeats();

            if (availableSeats.Count == 0)
                return true;

            return false;
        }

        public string convertSeatChartToJSON()
        {
            return JsonSerializer.Serialize(seatChart);
        }

        public void loadSeatChart(string fileName)
        {
            var json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + fileName);

            if (Utils.isValidJson(json))
            {
                try
                {
                    List<Seat> loadedSeats = JsonSerializer.Deserialize<List<Seat>>(json);

                    if (isLoadedSeatChartValid(loadedSeats))
                        seatChart = loadedSeats;

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private bool isLoadedSeatChartValid(List<Seat> loadedSeats)
        {
            if (loadedSeats.Count != seatChart.Count)
                return false;

            var i = 0;

            while (i < loadedSeats.Count)
            {
                if ((loadedSeats[i].row != seatChart[i].row) || (loadedSeats[i].column != seatChart[i].column))
                    return false;

                i++;
            }

            return true;
        }

    }
}
