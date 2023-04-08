using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Shouldly;
using VimeoDotNet.Models;
using VimeoDotNet.Net;
using VimeoDotNet.Parameters;

namespace VimeoDotNet.Tests
{
    public static class TestHelper
    {
        /// <summary>
        /// Execute method with test video
        /// </summary>
        /// <param name="client">Vimeo client</param>
        /// <param name="action">Test action with Clip Id</param>
        /// <returns>The result task</returns>
        public static async Task WithTempVideo(this IVimeoClient client,  Func<long, Task> action)
        {
            
        }
        
        /// <summary>
        /// Execute action with test album
        /// </summary>
        /// <param name="client">Vimeo client</param>
        /// <param name="action">Test action with clip Id</param>
        /// <returns>The result task</returns>
        public static async Task WithTestAlbum(this IVimeoClient client, Func<long, Task> action)
        {
            
        }
    }
}