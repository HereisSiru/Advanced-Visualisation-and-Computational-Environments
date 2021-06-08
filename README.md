# Find Fun 'In Quarantine'
### An AR & Unity project

I made a virtual friend Kirby (星のカービィ) in my 'In Quarantine' APP.

She will appear when I feel upset or down to accompany me in this hard Covid quarantine time.

<img width="360" alt="in app界面" src="https://user-images.githubusercontent.com/69792837/121137371-29013b80-c869-11eb-8f4b-4229fc134273.png">

* Game Demo Video: https://youtu.be/XpMvlw15VoQ


* Iterative Design Recording：https://youtu.be/Cz1tZ5khXWk





### Why I made it?



<img width="585" alt="Screenshot 2021-06-08 at 14 54 50" src="https://user-images.githubusercontent.com/69792837/121137712-85fcf180-c869-11eb-8844-1c68c63e251d.png">



Recently, I come back to China Mainland from the UK. According to the current policy, I have to have a quarantine time in Shanghai local Hotel. 


All things in the hotel are stranger to me and I have no choice. I need to stay in this hotel room for at least 14 days. I have no accompanies except my smart devices -- MY PHONES, MY MACBOOK, MY IPAD AND MY AIRPODS. 

It's harder than I think because I cannot go out. Initially, I was sent to a hotel in Shanghai's Changning district, but on the fourth day I was told there were COVID-infected people near me on the plane and I must be transferred to a more rigorous hotel immediately. I swear it's been an extremely stressful and terrible morning. 

I have never been to jail but I suppose there is some similarities. I have to create some fun by myself in this hotel room.

Through this semester's study, I have developed a strong interest in AR. So I made this project. The aiming of this project is I can get some fun in this boring and hard quarantine time. 

I made a virtual friend Kirby (星のカービィ) in my 'In Quarantine' APP. She will appear when I feel upset or down to accompany me in this hard Covid quarantine time.




### I recorded some study process and iterative design in readme file.

* Plane detection

* Use built-in functionality to detect horizontal and vertical planes

* Raycasting


#### see all details in added files or check Iterative Design Recording：https://youtu.be/Cz1tZ5khXWk



 ``` 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SimpleRaycast : MonoBehaviour
{
   public ARRaycastManager raycastManager;

   public GameObject objectToPlacePrefab;

   public Camera raycastCamera;

   private GameObject objectInstance;

   private List<ARRaycastHit> hits = new List<ARRaycastHit>();



    // Update is called once per frame
    void Update()
    {
        if (raycastManager == null)
             return;

        if (raycastCamera == null)
             return;

        if (Input.GetMouseButtonDown(0))
        {
             Ray ray = raycastCamera. ScreenPointToRay(Input. mousePosition);
             if(raycastManager.Raycast(ray, hits, UnityEngine.XR.ARSubsystems. TrackableType.Planes))
             {
                 Pose pose = hits[0].pose;
                 if (objectInstance == null)
                 {
                      objectInstance = Instantiate<GameObject>(objectToPlacePrefab, pose.position, pose.rotation);
                 }
                 else
                 {
                     objectInstance.transform.SetPositionAndRotation(pose.position, pose.rotation);
                 }
             }

        }
         
    }
}


 ``` 

