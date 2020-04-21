using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsumeStudentsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsumeStudentsAPI.Controllers {

    public class StudentsController : Controller {
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<Student> Students { get; set; }

        const string BASE_URL = "https://api.azurewebsites.net/";

        public StudentsController (IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index () {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/students");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                Students = await JsonSerializer.DeserializeAsync<IEnumerable<Student>> (responseStream);
            } else {
                Students = Array.Empty<Student> ();
            }

            return View (Students);
        }

        public IActionResult Create () {
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind ("studentId,firstName,lastName,school")] Student student) {
            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (student), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Post;
                message.RequestUri = new Uri ($"{BASE_URL}api/students");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync (message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (student);
        }

        public async Task<IActionResult> Details (string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/students/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Student student = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                student = await JsonSerializer.DeserializeAsync<Student> (responseStream);
            }

            if (student == null)
                return NotFound ();

            return View (student);

        }

        public async Task<IActionResult> Delete (string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/students/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Student student = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                student = await JsonSerializer.DeserializeAsync<Student> (responseStream);
            }

            if (student == null)
                return NotFound ();

            return View (student);

        }

        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id) {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Delete;
            message.RequestUri = new Uri ($"{BASE_URL}api/students/{id}");

            HttpClient client = _clientFactory.CreateClient ();
            HttpResponseMessage response = await client.SendAsync (message);

            var result = await response.Content.ReadAsStringAsync ();

            return RedirectToAction (nameof (Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}api/students/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            Student student = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                student = await JsonSerializer.DeserializeAsync<Student> (responseStream);
            } 

            if (student == null)
                return NotFound ();

            return View (student);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind ("studentId,firstName,lastName,school")] Student student) {
            if (id != student.studentId)
                return NotFound ();

            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (student), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Put;
                message.RequestUri = new Uri ($"{BASE_URL}api/students/{id}");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync(message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (student);
        }
    }

}