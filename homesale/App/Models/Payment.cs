using System;

namespace homesale.App.Models
{
    class Payment : Model<Payment>
    {
        public long id;

        public string description;
        public double price;

        public DateTime date;
        public DateTime date_paid;
    }
}
