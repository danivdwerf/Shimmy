﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : Photon.MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private float runSpeed;
    private float runScaler;

    private Animator anim;
    private int _velocityAnim;

    private Rigidbody rigid;
    private Vector3 movement;

    private Camera mainCam;

    private void Start()
    {
        runScaler = 0;
        movement = Vector3.zero;
       
        this.anim = this.GetComponent<Animator>();
        this._velocityAnim = Animator.StringToHash("velocity");

        rigid = GetComponent<Rigidbody> ();
    }

    private void Update()
    {
        if (!photonView.isMine)
            return;
        
        if (this.mainCam == null)
            this.mainCam = this.GetComponent<PlayerCam>().PlayerCamera;
        
        var x = Input.GetAxisRaw (Controller.LeftStickX) * speed;
        var z = Input.GetAxisRaw (Controller.LeftStickY) * speed;
        if ((z!=0 || x!=0) && Input.GetButton(Controller.Circle))
            runScaler = runSpeed;
        else
            runScaler = 1;

        movement = new Vector3 (x, 0, z);
    }

    private void FixedUpdate()
    {
        if (!photonView.isMine)
            return;
        
        if (this.mainCam == null)
            return;
        
        this.movement *= runScaler;
        Vector3 velocity = mainCam.transform.TransformDirection(movement) * Time.fixedDeltaTime;
        velocity.y = 0.0f;

        rigid.MovePosition(rigid.position + velocity);
        anim.SetFloat(_velocityAnim, movement.sqrMagnitude);
        if(velocity != Vector3.zero)
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.1f);
    }
}