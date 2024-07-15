using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventLibrary
{
    // ���׸� UnityEvent Ŭ���� ����
    [Serializable]
    public class GenericEvent<T> : UnityEvent<T> { }

    public class EventManager<E> where E : Enum
    {
        // �̺�Ʈ �̸��� �ش� UnityEvent�� �����ϴ� ��ųʸ�
        private static readonly Dictionary<E, UnityEventBase> EventDictionary = new Dictionary<E, UnityEventBase>();
        // ������ �������� ���� ��ü
        private static readonly object LockObj = new object();

        // �Ű������� ���� UnityAction �����ʸ� �߰��ϴ� �޼���
        public static void StartListening(E eventName, UnityAction listener)
        {
            AddListener(eventName, listener);
        }

        // ���׸� �Ű������� ���� UnityAction �����ʸ� �߰��ϴ� �޼���
        public static void StartListening<T>(E eventName, UnityAction<T> listener)
        {
            AddListener(eventName, listener);
        }

        // �Ű������� ���� UnityAction �����ʸ� �����ϴ� �޼���
        public static void StopListening(E eventName, UnityAction listener)
        {
            RemoveListener(eventName, listener);
        }

        // ���׸� �Ű������� ���� UnityAction �����ʸ� �����ϴ� �޼���
        public static void StopListening<T>(E eventName, UnityAction<T> listener)
        {
            RemoveListener(eventName, listener);
        }

        // �Ű������� ���� �̺�Ʈ�� Ʈ�����ϴ� �޼���
        public static void TriggerEvent(E eventName)
        {
            InvokeEvent(eventName);
        }

        // ���׸� �Ű������� ���� �̺�Ʈ�� Ʈ�����ϴ� �޼���
        public static void TriggerEvent<T>(E eventName, T parameter)
        {
            InvokeEvent(eventName, parameter);
        }

        // �̺�Ʈ�� �������� ������ �����Ͽ� ��ȯ�ϴ� �޼���
        private static TEvent GetOrCreateEvent<TEvent>(E eventName) where TEvent : UnityEventBase, new()
        {
            if (!EventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent = new TEvent();
                EventDictionary.Add(eventName, thisEvent);
            }
            return thisEvent as TEvent;
        }

        // �̺�Ʈ�� ��� ������ ��ųʸ����� �����ϴ� �޼���
        private static void RemoveEventIfEmpty(E eventName, UnityEventBase thisEvent)
        {
            if (thisEvent.GetPersistentEventCount() == 0)
            {
                EventDictionary.Remove(eventName);
            }
        }

        // �����ʸ� �߰��ϴ� �޼���
        private static void AddListener<T>(E eventName, UnityAction<T> listener)
        {
            lock (LockObj)
            {
                GenericEvent<T> genericEvent = GetOrCreateEvent<GenericEvent<T>>(eventName);
                genericEvent.AddListener(listener);
            }
        }

        private static void AddListener(E eventName, UnityAction listener)
        {
            lock (LockObj)
            {
                UnityEvent unityEvent = GetOrCreateEvent<UnityEvent>(eventName);
                unityEvent.AddListener(listener);
            }
        }

        // �����ʸ� �����ϴ� �޼���
        private static void RemoveListener<T>(E eventName, UnityAction<T> listener)
        {
            lock (LockObj)
            {
                if (EventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is GenericEvent<T> genericEvent)
                {
                    genericEvent.RemoveListener(listener);
                    RemoveEventIfEmpty(eventName, genericEvent);
                }
            }
        }

        private static void RemoveListener(E eventName, UnityAction listener)
        {
            lock (LockObj)
            {
                if (EventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is UnityEvent unityEvent)
                {
                    unityEvent.RemoveListener(listener);
                    RemoveEventIfEmpty(eventName, unityEvent);
                }
            }
        }

        // �̺�Ʈ�� Ʈ�����ϴ� �޼���
        private static void InvokeEvent<T>(E eventName, T parameter)
        {
            lock (LockObj)
            {
                try
                {
                    if (EventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is GenericEvent<T> genericEvent)
                    {
                        genericEvent.Invoke(parameter);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error triggering event {eventName} with parameter {parameter}: {e.Message}");
                }
            }
        }

        private static void InvokeEvent(E eventName)
        {
            lock (LockObj)
            {
                try
                {
                    if (EventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is UnityEvent unityEvent)
                    {
                        unityEvent.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error triggering event {eventName}: {e.Message}");
                }
            }
        }
    }
}
