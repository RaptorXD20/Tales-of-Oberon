using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    float currentEventCooldown = 0;

    public EventData[] events;

    //il tempo da spettare prima che l'evento diventa attivo
    public float firstTriggerDelay = 180f;

    //quanto tempo aspettare fra ogni evento
    public float triggerInterval = 30f;

    public static EventManager instance;

    [System.Serializable]
    public class Event{
        public EventData data;
        public float duration, cooldown = 0;
    }

    List<Event> runningEvents = new List<Event>(); //questi sono gli eventi attualmente attivi

    PlayerStats[] allPlayers;

    void Start(){
        if(instance) Debug.LogWarning("There is more then 1 Spawn Manager in the Scene! Please remove the extras");
        instance = this;
        currentEventCooldown = firstTriggerDelay > 0? firstTriggerDelay : triggerInterval;
        allPlayers = FindObjectsOfType<PlayerStats>();
    }

    void Update(){
        //aspetta che il cooldown per far attivare un evento
        currentEventCooldown -= Time.deltaTime;
        if(currentEventCooldown <= 0){
            //prendi il un evento e prova a farlo partire
            EventData e = GetRandomEvent();
            if(e && e.CheckIfWillHappen(allPlayers[Random.Range(0, allPlayers.Length)])){
                runningEvents.Add(new Event{data = e, duration = e.duration});
            }

            //setata il cooldawn dell'evento
            currentEventCooldown = triggerInterval;
        }

        //Eventi che volgiamo rimuovere
        List<Event> toRemove = new List<Event>();

        //controlla ogni cooldown degli eventi per vedere se sono ancora attivi
        foreach(Event e in runningEvents){
            //riduci il cooldown
            e.duration -= Time.deltaTime;

            if(e.duration <= 0){
                toRemove.Add(e);
                continue;
            }

            //riduci il cooldown corrente
            e.cooldown -= Time.deltaTime;
            if(e.cooldown <= 0){
                //prendi il player per indirizzare l'evento
                // poi resetta il cooldown
                e.data.Activate(allPlayers[Random.Range(0, allPlayers.Length)]);
                e.cooldown = e.data.GetSpawnInterval();
            }
        }

        //rimuovi tutti gli eventi che sono scaduti
        foreach(Event e in toRemove) runningEvents.Remove(e);
    }

    public EventData GetRandomEvent(){
        //se non ci sono eventi, ritorna null
        if(events.Length <= 0) return null;

        //prendi una lista di eventi
        List<EventData> possibleEvents = new List<EventData>(events);

        //scegli un evento randomico e vedi se lo puoi usare
        //continua a farlo finche non trovi un evento che puoi attivare
        EventData result = possibleEvents[Random.Range(0, possibleEvents.Count)];
        while(!result.IsActive()){
            possibleEvents.Remove(result);
            if(possibleEvents.Count > 0){
                result = events[Random.Range(0, possibleEvents.Count)];
            }else{
                return null;
            }
        }

        return result;
    }
}
