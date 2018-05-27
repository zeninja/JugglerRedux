using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
	// private dictionary to hold the events 
	private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

	private static EventManager instance = null;
	public static EventManager GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		if (instance == null) {
			instance = this;
			instance.Init();
		} else {
			if (this != instance) {
				Destroy(gameObject);
			}
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Init()
	{ 
	}

	public static void StartListening(string eventName, UnityAction listener, MonoBehaviour mb = null)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) 		{
			thisEvent.AddListener(listener);
		}
		else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
			//Debug.Log("STARTED LISTENING: " + eventName);
		}

		if(mb != null) {
			Debug.Log("STARTED LISTENING: " + eventName);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if ( instance == null)
			return;

		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName, bool debug = false, MonoBehaviour mb = null)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();

			if(debug) {
				string message;
				message = "TRIGGERING EVENT: " + eventName;
				if(mb != null) {
					message += "\nCalled by: " + mb;
				}
				Debug.Log(message);
			}
		}
	}
}