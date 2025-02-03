using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    //public AnimationController animationController;

    private Vector3 _input;

    [SerializeField] Camera cam;

    public LayerMask mask;

    public int interpolationFramesCount = 45;
    int elaspedFrames = 0;


    public void Update()
    {
        PlayerMovementInput();
        
    }

    public void FixedUpdate()
    {
        Move();
    }

    void PlayerMovementInput()
    {
        GatherInput();
        Look();
    
    }


    public void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal") + (Input.GetAxisRaw("Vertical") / 2 * 1.6f), 0, (Input.GetAxisRaw("Horizontal") / 2 * -1.6f) + Input.GetAxisRaw("Vertical") );
        if(_input.sqrMagnitude == 0)
        {
            //animationController.PlayerStopWalkingAnimation();
        }

    }

    public void Look()
    {
        if (_input != Vector3.zero)
        {

            var relative = (transform.position + _input.ToIso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            Debug.Log("the rotation amount" + rot.ToString());

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * (Time.deltaTime * 2.8f));
        }
    }

    public void Move()
    {
        //animationController.PlayerWalkingAnimation();

        _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);


        //cam.transform.position = _rb.transform.position;
    }

    void CursorMovement()
    {
        //Draw the Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);


        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, mask))
        {
            float interpolationRatio = (float)elaspedFrames / (float)interpolationFramesCount;
            Debug.Log("working");
            //_rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
            _rb.transform.position = _rb.transform.position + (Vector3.Lerp(_rb.transform.position, hit.point, interpolationRatio)) * _speed * Time.deltaTime;
            //_rb.transform.position = Vector3.Lerp(_rb.transform.position, hit.point, interpolationRatio);

            elaspedFrames = (elaspedFrames + 1) % (interpolationFramesCount + 1);

            hit.transform.GetComponent<Renderer>().material.color = Color.red;


        }
    }
}
