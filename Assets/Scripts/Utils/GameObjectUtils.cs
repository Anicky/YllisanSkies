using UnityEngine;
using System.Linq;

public class GameObjectUtils
{

    public static GameObject searchByNameInList(GameObject[] list, string name)
    {
        return list.Where(obj => obj.name == name).SingleOrDefault();
    }
}
