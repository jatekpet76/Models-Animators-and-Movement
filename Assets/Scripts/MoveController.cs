using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour { 
    [SerializeField] float _turnSpeed = 20f;
    [SerializeField] float _speed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        
        var addToPosition = m_Movement * _speed * Time.deltaTime;

        Debug.Log(string.Format("{0}", addToPosition.ToString("F4")));

        SetVelocity(horizontal, vertical);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, _turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void SetVelocity(float horizontal, float vertical)
    {
        m_Animator.SetFloat("Velocity", Mathf.Abs(vertical) > 0? Mathf.Abs(vertical): Mathf.Abs(horizontal));
    }

    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAnimatorMove.html
    // Callback for processing animation movements for modifying root motion.
    // This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK.
    void OnAnimatorMove()
    {
        //  * m_Animator.deltaPosition.magnitude - John Lemon rootMotion???
        m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Movement * _speed * Time.deltaTime));
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
