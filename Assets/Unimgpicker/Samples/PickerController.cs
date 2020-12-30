using UnityEngine;
using System.Collections;

namespace Kakera
{
    public class PickerController : MonoBehaviour
    {
        [SerializeField]
        private Unimgpicker imagePicker;
        [SerializeField]
        private UnimgpickerCamera imageCamera;

        //[SerializeField]
        public GameObject myObject;

        private PrimitiveType meshType = PrimitiveType.Cube;

        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, myObject.GetComponent<MeshRenderer>()));
            };
            imageCamera.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, myObject.GetComponent<MeshRenderer>()));
            };
        }
        IEnumerator Start()
        {
            string url = "https://google.com";
            WWW www = new WWW(url);
            yield return www;
            MeshRenderer renderer = myObject.GetComponent<MeshRenderer>();
            renderer.material.mainTexture = www.texture;
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "unimgpicker", 1024);
        }

        public void/*IEnumerator*/ OnPressShowCamera()
        {
            /*yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            WebCamTexture webCamTexture = new WebCamTexture();
            imageRenderer.material.mainTexture = webCamTexture;
            webCamTexture.Play();
            //imageCamera.Show("Capture Image", "unimgpicker", 1024);
            */
            int maxSize = 1024;
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create a Texture2D from the captured image
                    Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    // Assign texture to a temporary quad and destroy it after 5 seconds
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    Material material = quad.GetComponent<Renderer>().material;
                    if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                        material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    material.mainTexture = texture;

                    Destroy(quad, 5f);

                    // If a procedural texture is not destroyed manually, 
                    // it will only be freed after a scene change
                    Destroy(texture, 5f);
                }
            }, maxSize);

            Debug.Log("Permission result: " + permission);
        }

        private IEnumerator LoadImage(string path, MeshRenderer output)
        {
            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

            output.material.mainTexture = texture;
        }

        public void ToCube()
        {
            //if (meshType == PrimitiveType.Cube)
            //{
            //    return;
            //}

            //meshType = PrimitiveType.Cube;
            //GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //newObject.transform.Translate(myObject.transform.position);
            //newObject.transform.localScale = new Vector3(3.6f, 3.6f, 3.6f);
            //newObject.GetComponent<MeshRenderer>().material = myObject.GetComponent<MeshRenderer>().material;
            //Rotator rotator = newObject.AddComponent<Rotator>();
            //rotator.rotationVector = new Vector3(10, 30, 10);
            //Destroy(myObject);
            //myObject = newObject;
            ChangePrimitive(PrimitiveType.Cube);
        }

        public void ToSphere()
        {
            ChangePrimitive(PrimitiveType.Sphere);
        }

        public void ToCylinder()
        {
            ChangePrimitive(PrimitiveType.Cylinder);
        }

        public void ToCapsule()
        {
            ChangePrimitive(PrimitiveType.Capsule);
        }

        protected void ChangePrimitive(PrimitiveType primType)
        {
            if (meshType == primType)
            {
                return;
            }

            meshType = primType;
            GameObject newObject = GameObject.CreatePrimitive(meshType);
            newObject.transform.Translate(myObject.transform.position);
            newObject.transform.localRotation = myObject.transform.localRotation;
            newObject.transform.localScale = new Vector3(3.6f, 3.6f, 3.6f);
            newObject.GetComponent<MeshRenderer>().material = myObject.GetComponent<MeshRenderer>().material;
            Rotator rotator = newObject.AddComponent<Rotator>();
            rotator.rotationVector = new Vector3(10, 30, 10);
            Destroy(myObject);
            myObject = newObject;
        }
    }
}