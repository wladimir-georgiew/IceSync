using System.Text;
using IceSync.Common.Contracts;
using Newtonsoft.Json;

namespace IceSync.Common.Implementations;

public class Requester(HttpClient httpClient)
{
	public async Task<BaseResult<T>> SendRequestAsync<T>(HttpMethod method, string url, object? data = null)
	{
		var request = new HttpRequestMessage(method, url);
		
		if (data is not null)
		{
			request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
		}

		HttpResponseMessage response;

		try
		{
			response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
		}
		catch (Exception e)
		{
			return new BaseResult<T>
			{
				Success = false,
				Error = new Error()
				{
					Code = 500,
					Message = e.Message
				}
			};
		}

		if (!response.IsSuccessStatusCode)
		{
			string content = request.Content is null
				? ""
				: await request.Content.ReadAsStringAsync();

			return new BaseResult<T>()
			{
				Success = false,
				Error = new Error()
				{
					Code = (int)response.StatusCode,
					Message = $"Request failed with status code {response.StatusCode} | {content}"
				}
			};
		}

		T? responseData;
		
		try
		{
			var responseDataString = await response.Content.ReadAsStringAsync();
			responseData = JsonConvert.DeserializeObject<T>(responseDataString);
		}
		catch (Exception ex)
		{
			return new BaseResult<T>()
			{
				Success = false,
				Error = new Error()
				{
					Message = $"Failed to parse response. {ex.Message}",
					Code = 500
				}
			};
		}

		return new BaseResult<T>
		{
			Success = true,
			Data = responseData,
		};
	}

	public async Task<BaseResult> SendRequestAsync(HttpMethod method, string url, object? data = null)
	{
		var request = new HttpRequestMessage(method, url);
		
		if (data is not null)
		{
			request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
		}

		HttpResponseMessage response;

		try
		{
			response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
		}
		catch (Exception e)
		{
			return new BaseResult
			{
				Success = false,
				Error = new Error()
				{
					Code = 500,
					Message = e.Message
				}
			};
		}
		
		BaseResponse? responseData;
		
		try
		{
			var responseDataString = await response.Content.ReadAsStringAsync();
			responseData = JsonConvert.DeserializeObject<BaseResponse>(responseDataString);
		}
		catch
		{
			return new BaseResult()
			{
				Success = false,
				Error = new Error()
				{
					Message = "Failed to parse response",
					Code = 500
				}
			};
		}

		Error? error = null;
		if (!string.IsNullOrWhiteSpace(responseData.Error))
		{
			error = new()
			{
				Code = (int)response.StatusCode,
				Message = responseData.Error,
			};
		}
		
		return new BaseResult()
		{
			Success = response.IsSuccessStatusCode,
			Error = error,
		};
	}
}