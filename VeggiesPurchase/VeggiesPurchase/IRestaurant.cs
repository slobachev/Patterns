namespace VeggiesPurchase
{
    internal interface IRestaurant
    {
        Veggies Veggies { get; set; }

        void Update(Veggies veggies);
    }
}