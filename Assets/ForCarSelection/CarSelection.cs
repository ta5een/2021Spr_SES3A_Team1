using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCar;

    private void Awake()
    {
        SelectCar(0);
    }

    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != transform.childCount-1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
            if(transform.GetChild(i).gameObject.activeSelf == true)
            {
                GameObject.Find("PlayerPref").GetComponent<SaveSelectedCar>().setCar(transform.GetChild(i).gameObject.transform.name);
            }
        }
    }

    public void ChangeCar(int _change)
    {
        FindObjectOfType<AudioManager>().Play("Swoosh");
        currentCar += _change;
        SelectCar(currentCar);
    }
}
