﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Common;

/// <summary>
/// ResourcesLoader 本地资源加载器
/// </summary>
public class ResourcesLoader : IResourceLoader
{
	

	/// <summary>
	/// 同步加载动更资源
	/// </summary>
	/// <returns>资源内容</returns>
	/// <param name="fileName">文件名称</param>
	/// <typeparam name="T">资源类型</typeparam>
	public override T LoadAsset<T> (string fileName)
	{
		string path = FileUtils.getLocalPath(fileName);

		return Resources.Load<T> (path);
	}

	/// <summary>
	/// 异步加载动更资源
	/// </summary>
	/// <returns>资源内容</returns>
	/// <param name="fileName">文件名称</param>
	/// <typeparam name="T">资源类型</typeparam>
	public override T LoadAssetAsync<T> (string fileName)
	{
		string path = FileUtils.getLocalPath(fileName);
		ResourceRequest request = Resources.LoadAsync<T> (path);
		if (request.isDone) {
			return (T)request.asset;
		}
		return null;
	}

	/// <summary>
	/// 读取文本文件内容
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="fileName">File name.</param>
	public override string LoadText (string fileName)
	{
		throw new NotImplementedException ();
	}
}

