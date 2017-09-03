namespace RaverSoft.YllisanSkies.Characters
{
    public enum BehaviourList
    {
        Cruel,
        Rancorous
    }

    public class Behaviour
    {
        private string name;

        public Behaviour(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }
    }
}