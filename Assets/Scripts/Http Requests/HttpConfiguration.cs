using UnityEngine;
using System.Collections;

public class HttpConfiguration{

	string _name;
	string _contentType;
	byte[] _content;

	string _filename;
	bool _hasFilename = false;

	public HttpConfiguration(string name, string filename, string contentType, byte[] content)
	{
		_hasFilename = true;

		_name = name;
		_filename = filename;
		_contentType = contentType;
		_content = content;
	}

	public HttpConfiguration(string name, string contentType, byte[] content)
	{
		_name = name;
		_contentType = contentType;
		_content = content;
	}

	public string getName()
	{
		return _name;
	}

	public string getContentType()
	{
		return _contentType;
	}

	public byte[] getContent()
	{
		return _content;
	}

	public string getFileName()
	{
		return _filename;
	}

	public bool hasFileName()
	{
		return _hasFilename;
	}
}