﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;

namespace Izmir
{
	public class PostClient
	{
		public PostClient () {
		}

		/*public async Task<Post[]> GetPostsAsync () {

			var client = new HttpClient ();

			client.BaseAddress = new Uri("http://www.barcelonaismedia.com/");

			var response = await client.GetAsync("?json=get_posts&date_format=Y-m-d&include=id,title,thumbnail,date,content,url,excerpt&count=10");

			var postJson = response.Content.ReadAsStringAsync().Result;

			var rootobject = JsonConvert.DeserializeObject<Rootobject>(postJson);

			return rootobject.posts;

		}*/

		public async Task<List<Post>> GetPosts ()
		{ 
			var rootobject = new Rootobject();

			using (var httpClient = CreateClient ()) {
				var response = await httpClient.GetAsync ("?json=get_posts&date_format=Y-m-d&include=id,title,thumbnail,date,author,content,url,excerpt&count=5&offset=2").ConfigureAwait(false);
				if (response.IsSuccessStatusCode) {
					var json = await response.Content.ReadAsStringAsync ().ConfigureAwait (false);
					if (!string.IsNullOrWhiteSpace (json)) {
						rootobject = await Task.Run (() => 
							JsonConvert.DeserializeObject<Rootobject> (json)
						).ConfigureAwait (false);
					}
				}
			}

			return rootobject.posts.ToList();
		}

		private const string ApiBaseAddress = "http://kzine.se/";
		private HttpClient CreateClient ()
		{
			var httpClient = new HttpClient 
			{ 
				BaseAddress = new Uri(ApiBaseAddress)
			};

			httpClient.Timeout = TimeSpan.FromMilliseconds (80000);

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return httpClient;
		}
	}
}

