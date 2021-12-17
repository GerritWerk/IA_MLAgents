using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.CommunicatorObjects;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToAgents : Agent
{
	[SerializeField] private Transform targer;
	[SerializeField] private Material Win;
	[SerializeField] private Material Lose;
	[SerializeField] private MeshRenderer FloorMesh;
	[SerializeField] private KeyCode move;
	public override void OnEpisodeBegin()
	{
		transform.localPosition = new Vector3(Random.Range(-1.8f,+1.66f),0,Random.Range(+3f,+2f));
		targer.localPosition = new Vector3(Random.Range(-2.3f, +1.22f), 0, Random.Range(-1f, +1f));
	}
	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.localPosition);
		sensor.AddObservation(targer.localPosition);
	}
	public override void OnActionReceived(ActionBuffers actions)
	
	{
		float moveX = actions.ContinuousActions[0];
		float moveZ = actions.ContinuousActions[1];

		float moveSpeed = 3f;
		transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{

		ActionSegment<float> continuousAction = actionsOut.ContinuousActions;

		continuousAction[0] = Input.GetAxisRaw("Vertical");
		continuousAction[1] = Input.GetAxisRaw("Horizontal");

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Goal>(out Goal goal))
		{
			SetReward(+1f);
			FloorMesh.material = Win;
			EndEpisode();
		}
		if (other.TryGetComponent<wall>(out wall wall))
		{
			SetReward(-1f);
			FloorMesh.material = Lose;
			EndEpisode();
		}
	}

}
