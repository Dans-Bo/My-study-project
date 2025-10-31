
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class Blackboard : MonoBehaviour
    {
        private Dictionary<string, object> data = new();
        public Dictionary<string, object> Data { get { return data; } }
        //private  static Dictionary<string, object> playerPos = new();

        /// <summary>
        /// 读取字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (data.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return default;
        }

        /// <summary>
        /// 添加进字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            data[key] = value;
        }

        /// <summary>
        /// 删除特定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            bool hasKey = Data.ContainsKey(key);
            if (hasKey)
            {
                Data.Remove(key);
            }
            return hasKey;
        }
        /// <summary>
        /// 清空所有位置数据
        /// </summary>
        public  void ClearAllPlayerPos()
        {
            data.Clear();
        }
            
        /* public static T GetPlayerPos<T>(string key)
        {
            if (playerPos.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return default;
        }

        public static void SetPlayerPos<T>(string key, T value)
        {
            playerPos[key] = value;
        }
        /// <summary>
        /// 删除特定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemovePlayerPos(string key)
        {
            return playerPos.Remove(key);
        }

        /// <summary>
        /// 清空所有位置数据
        /// </summary>
        public static void ClearAllPlayerPos()
        {
            playerPos.Clear();
        }

        /// <summary>
        /// 检查是否存在某个键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasPlayerPos(string key)
        {
            return playerPos.ContainsKey(key);
        } */
    }
}
