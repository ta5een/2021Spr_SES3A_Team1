//Created by Ryan Dawson 13270006
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countdown : MonoBehaviour
{
    private Transform[] children;
    private Vector3 numOffset;
    private Vector3 gOffset;
    private Vector3 oOffset;
    private Vector3 exclaimOffset;
    private Color threeColAlpha;
    private Color twoColAlpha;
    private Color oneColAlpha;
    private Color goColAlpha;
    private Color outlineAlpha;
    private float timer;
    private Coroutine threeRoutine;
    private Coroutine twoRoutine;
    private Coroutine oneRoutine;
    private Coroutine goRoutine;
    [SerializeField]
    private GameObject three;
    [SerializeField]
    private Color threeCol;
    [SerializeField]
    private GameObject two;
    [SerializeField]
    private Color twoCol;
    [SerializeField]
    private GameObject one;
    [SerializeField]
    private Color oneCol;
    [SerializeField]
    private GameObject go;
    [SerializeField]
    private Color goCol;
    [SerializeField]
    private Color outline;
    [SerializeField]
    private float timeBetweenNums;
    [SerializeField]
    private float startBuffer;
    private FuelSystem fuelBar;
    private CarController carController;
    private GameObject pauseBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        //Disable car controller script, fuel system and pause btn before countdown begins
        carController = GameObject.FindWithTag("Player").GetComponent<CarController>();
        fuelBar = GameObject.Find("FuelBar").GetComponent<FuelSystem>();
        pauseBtn = GameObject.FindWithTag("PauseBtn");
        carController.enabled = false;
        fuelBar.enabled = false;
        pauseBtn.SetActive(false);
        
        //Copying colors but changing alpha value to 0
        threeColAlpha = new Color(threeCol.r, threeCol.g, threeCol.b, 0.0f);
        twoColAlpha = new Color(twoCol.r, twoCol.g, twoCol.b, 0.0f);
        oneColAlpha = new Color(oneCol.r, oneCol.g, oneCol.b, 0.0f);
        goColAlpha = new Color(goCol.r, goCol.g, goCol.b, 0.0f);
        outlineAlpha = new Color(outline.r, outline.g, outline.b, 0.0f);

        //Assigning color to each gameobjects body
        three.GetComponent<MeshRenderer>().materials[0].color = threeCol;
        two.GetComponent<MeshRenderer>().materials[0].color = twoCol;
        one.GetComponent<MeshRenderer>().materials[1].color = oneCol;

        //Assigning color to each gameobjects outline
        three.GetComponent<MeshRenderer>().materials[1].color = outline;
        two.GetComponent<MeshRenderer>().materials[1].color = outline;
        one.GetComponent<MeshRenderer>().materials[0].color = outline;
        FindObjectOfType<AudioManager>().Play("Start sound");

        //Looping through GO! prefabs children and applying color as well as disabling renders for until its needed
        foreach (MeshRenderer r in go.GetComponentsInChildren<MeshRenderer>()){
            r.materials[1].color = goCol;
            r.materials[0].color = outline;
            r.enabled = false;
        }

        //Disabling renders for numbers until they are needed
        two.GetComponent<MeshRenderer>().enabled = false;
        one.GetComponent<MeshRenderer>().enabled = false;
        

        children = go.GetComponentsInChildren<Transform>();
        gOffset = children[2].position - gameObject.transform.position;
        oOffset = children[3].position - gameObject.transform.position;
        exclaimOffset = children[1].position - gameObject.transform.position;
        numOffset = three.transform.position - gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Makes sure numbers/letters are in front of the camera
        three.transform.position = gameObject.transform.position + numOffset;
        two.transform.position = gameObject.transform.position + numOffset;
        one.transform.position = gameObject.transform.position + numOffset;
        children[1].position = gameObject.transform.position + exclaimOffset;
        children[2].position = gameObject.transform.position + gOffset;
        children[3].position = gameObject.transform.position + oOffset;

        //Keeps track of the current time by adding up the time between each frame
        timer += Time.deltaTime;

        //If within correct time frame "1" gamobjects mesh is enabled and begins fading 
        if((int)timer < (startBuffer+timeBetweenNums) && (int)timer >= startBuffer && threeRoutine == null){
            StartCoroutine(threeFade(timeBetweenNums));
        }
        //If within correct time frame "2" gamobject's mesh is enabled and begins fading 
        if((int)timer < (startBuffer+timeBetweenNums+timeBetweenNums) && (int)timer >= (startBuffer+timeBetweenNums) && twoRoutine == null){
            two.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(twoFade(timeBetweenNums));
        }
        //If within correct time frame "3" gamobject's mesh is enabled and begins fading 
        if((int)timer < (startBuffer+timeBetweenNums+timeBetweenNums+timeBetweenNums) && (int)timer >= (startBuffer+timeBetweenNums+timeBetweenNums) && oneRoutine == null){
            one.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(oneFade(timeBetweenNums));
        }
        //If within correct time frame "GO!" gamobject's mesh is enabled and begins fading 
        if((int)timer < (startBuffer+timeBetweenNums+timeBetweenNums+timeBetweenNums+timeBetweenNums) && (int)timer >= (startBuffer+timeBetweenNums+timeBetweenNums+timeBetweenNums) && goRoutine == null){
            foreach(MeshRenderer r in go.GetComponentsInChildren<MeshRenderer>()){
                r.enabled = true;
            }
            //Enables car controller script as "GO!" appears on screen
            // GameObject.Find("FuelBar").GetComponent<FuelSystem>().enabled = true;
            // GameObject.FindWithTag("Player").GetComponent<CarController>().enabled = true;
            // GameObject.FindWithTag("PauseBtn").SetActive(true);
            carController.enabled = true;
            fuelBar.enabled = true;
            pauseBtn.SetActive(true);
            
            StartCoroutine(goFade(timeBetweenNums));
        }
        //Disables this script when countdown is done
        if((int)timer > (startBuffer+timeBetweenNums+timeBetweenNums+timeBetweenNums+1) && goRoutine == null){
            this.enabled = false;
        }
    }

    //Coroutine for fading the number three
    IEnumerator threeFade(float duration){
        float counter = 0;
        while(counter < duration){
            //Calculations for quintic interpolation
            counter += Time.deltaTime;
            float t = counter/duration;
            t = t*t*t*t*t;
            //Lerp outline color and base color to reduce alpha values
            three.GetComponent<MeshRenderer>().materials[0].color = Color.Lerp(threeCol, threeColAlpha, t);
            three.GetComponent<MeshRenderer>().materials[1].color = Color.Lerp(outline, outlineAlpha, t);
            yield return null;
        }
        //Clean up
        yield return null;
        three.GetComponent<MeshRenderer>().enabled = false;
        threeRoutine = null;
    }

    //Coroutine for fading the number two
    IEnumerator twoFade(float duration){
        float counter = 0;
        while(counter < duration){
            //Calculations for quintic interpolation
            counter += Time.deltaTime;
            float t = counter/duration;
            t = t*t*t*t*t;
            //Lerp outline color and base color to reduce alpha values
            two.GetComponent<MeshRenderer>().materials[0].color = Color.Lerp(twoCol, twoColAlpha, t);
            two.GetComponent<MeshRenderer>().materials[1].color = Color.Lerp(outline, outlineAlpha, t);
            yield return null;
        }
        //Clean up
        yield return null;
        two.GetComponent<MeshRenderer>().enabled = false;
        twoRoutine = null;
    }

    //Coroutine for fading the number one
    IEnumerator oneFade(float duration){
        float counter = 0;
        while(counter < duration){
            //Calculations for quintic interpolation
            counter += Time.deltaTime;
            float t = counter/duration;
            t = t*t*t*t*t;
            //Lerp outline color and base color to reduce alpha values
            one.GetComponent<MeshRenderer>().materials[1].color = Color.Lerp(oneCol, oneColAlpha, t);
            one.GetComponent<MeshRenderer>().materials[0].color = Color.Lerp(outline, outlineAlpha, t);
            yield return null;
        }
        //Clean up        
        yield return null;
        one.GetComponent<MeshRenderer>().enabled = false;
        oneRoutine = null;
    }

    //Coroutine for fading the word GO!
    IEnumerator goFade(float duration){
        float counter = 0;
        while(counter < duration){
            //Calculations for quintic interpolation
            counter += Time.deltaTime;
            float t = counter/duration;
            t = t*t*t*t*t;
            //Lerp outline color and base color to reduce alpha values
            foreach(MeshRenderer r in go.GetComponentsInChildren<MeshRenderer>()){
                r.materials[1].color = Color.Lerp(goCol, goColAlpha, t);
                r.materials[0].color = Color.Lerp(outline, outlineAlpha, t);
            }
            yield return null;
        }
        //Clean up
        yield return null;        
        foreach(MeshRenderer r in go.GetComponentsInChildren<MeshRenderer>()){
           r.enabled = false;
        }    
        goRoutine = null;
    }
}