  // Smooth towards the target
     
     using UnityEngine;
     using System.Collections;
     
     public class CameraFollow : MonoBehaviour
     {
         public Transform target;
         public float smoothTime = 0.3F;
            public float roationSmoothTime;
         private Vector3 velocity = Vector3.zero;
     
         void Update()
         {
             // Define a target position above and behind the target transform
             Vector3 targetPosition =  target.position;  /*+ new Vector3 (0f,0f,-10f);  target.TransformPoint(new Vector3(0, 5, -10)); */
             Quaternion targetRotation = target.rotation;

        // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
           transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, roationSmoothTime);
         }
     }