using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Chaser1 : ActionNode
{

    private GameObject player;
    private GameObject guard;
    private Animator guardAnimator;
    public guardProp guardProp;

    public float vitesse_poursuite;
    public float vitesseRot_poursuite;
    public float precision_poursuite;

    protected override void OnStart()
    {
        player = GameObject.Find("Player");
        guard = context.gameObject;

        guardAnimator = guard.GetComponent<Animator>();
        guardProp = guard.GetComponent<guardProp>();

        vitesse_poursuite = 3f;
        vitesseRot_poursuite = 5f;
        precision_poursuite = 0.5f;
    }

    protected override void OnStop()
    {
        guardAnimator.SetBool("isRunning", false);
    }

    protected override State OnUpdate()
    {
        Poursuivre(player.transform);

        float distanceToPlayer = Vector3.Distance(guard.transform.position, player.transform.position);

        if (distanceToPlayer < 1.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return State.Running;
        }
        else
        {
            blackboard.moveToPosition = player.transform.position;
            guardAnimator.SetBool("isIdle", false);
            guardAnimator.SetBool("isRunning", true);
            return State.Running;
        }
    }

    public void Poursuivre(Transform Player)
    {
        NavMeshAgent agent = guard.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.ResetPath();

        Vector3 direction = Player.position - guard.transform.position;
        guard.transform.rotation = Quaternion.Slerp(guard.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * vitesseRot_poursuite);

        if (direction.magnitude > precision_poursuite)
        {
            guard.transform.Translate(0, 0, Time.deltaTime * vitesse_poursuite);
        }
    }
}
