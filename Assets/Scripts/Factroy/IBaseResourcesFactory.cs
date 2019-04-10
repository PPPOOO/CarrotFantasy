using UnityEngine;
using System.Collections;

public interface IBaseResourcesFactory <T>
{

    T GetSingleResources(string resourcePath);

}
