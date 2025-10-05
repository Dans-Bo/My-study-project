using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 输入缓冲
/// </summary>
public class InputBuffer
{
    public string inputName;
    public float bufferTime;
    public float bufferCounter;
    public void BufferInput()
    {
        bufferCounter = bufferTime;
    }

    // Update is called once per frame
    public void Update()
    {
        if (bufferCounter > 0)
        {
            bufferCounter -= Time.deltaTime;
        }
    }

    public bool IsBuffered()
    {
        return bufferCounter > 0;
    }

    public void Clear()
    {
        bufferCounter = 0;
    }
}

public class InputBufferSyetem
{
    private Dictionary<string, InputBuffer> buffers = new();

    // 注册一个新的输入类型到缓冲系统
    public void RegisterBuffer(string inputName, float bufferTime)
    {
        InputBuffer newBuffer = new()
        {
            // 设置实例的属性值
            inputName = inputName,
            bufferTime = bufferTime
        };
        // 将新实例添加到字典
        buffers[inputName] = newBuffer;

    }

    public void BufferInput(string inputName)
    {
        if (buffers.ContainsKey(inputName))
        {
            buffers[inputName].BufferInput();
        }
    }

    public void Update()
    {
        foreach (var buffer in buffers.Values)
        {
            buffer.Update();
        }
    }

    public bool IsBuffered(string inputName)
    {
        return buffers.ContainsKey(inputName) && buffers[inputName].IsBuffered();
    }

    public float GetBufferTimes(string inputName)
    {
        return buffers.ContainsKey(inputName) ? buffers[inputName].bufferCounter : 0;
    }

    public void ClearBuffer(string inputName)
    {
        if (buffers.ContainsKey(inputName))
        {
            buffers[inputName].Clear();
        }
    }
}
