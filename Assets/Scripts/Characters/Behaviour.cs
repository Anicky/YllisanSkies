namespace RaverSoft.YllisanSkies.Characters
{
    public enum BehaviourList
    {
        Cruel,
        Rancorous
    }

    public class Behaviour
    {
        private string id;
        private string name;

        public Behaviour(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }
    }
}