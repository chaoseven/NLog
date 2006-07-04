// 
// Copyright (c) 2004-2006 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

#if !NETCF
using System;
using System.Text;
using System.IO;
using NLog.Config;

namespace NLog.LayoutRenderers
{
    /// <summary>
    /// System special folder path (includes My Documents, My Music, Program Files, Desktop, and more)
    /// </summary>
    [LayoutRenderer("specialfolder")]
    [NotSupportedRuntime(Framework=RuntimeFramework.DotNetCompactFramework)]
    public class SpecialFolderLayoutRenderer : LayoutRenderer
    {
        private string _fileName = null;
        private string _directoryName = null;
        private System.Environment.SpecialFolder _folder;

        /// <summary>
        /// System special folder.
        /// </summary>
        /// <remarks>
        /// Full list of options is available at <a href="http://msdn2.microsoft.com/en-us/system.environment.specialfolder.aspx">MSDN</a>.
        /// The most common ones are:
        /// <ul>
        /// <li><b>ApplicationData</b> - roaming application data for current user.</li>
        /// <li><b>CommonApplicationData</b> - application data for all users.</li>
        /// <li><b>MyDocuments</b> - My Documents</li>
        /// <li><b>DesktopDirectory</b> - Desktop directory</li>
        /// <li><b>LocalApplicationData</b> - non roaming application data</li>
        /// <li><b>Personal</b> - user profile directory</li>
        /// <li><b>System</b> - System directory</li>
        /// </ul>
        /// </remarks>
        public System.Environment.SpecialFolder Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        /// <summary>
        /// The name of the file to be Path.Combine()'d with the directory name.
        /// </summary>
        public string File
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// The name of the directory to be Path.Combine()'d with the directory name.
        /// </summary>
        public string Dir
        {
            get { return _directoryName; }
            set { _directoryName = value; }
        }

        /// <summary>
        /// Returns the estimated number of characters that are needed to
        /// hold the rendered value for the specified logging event.
        /// </summary>
        /// <param name="logEvent">Logging event information.</param>
        /// <returns>The number of characters.</returns>
        /// <remarks>
        /// If the exact number is not known or
        /// expensive to calculate this function should return a rough estimate
        /// that's big enough in most cases, but not too big, in order to conserve memory.
        /// </remarks>
        protected internal override int GetEstimatedBufferSize(LogEventInfo logEvent)
        {
            return 32;
        }

        /// <summary>
        /// Renders the directory where NLog is located and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected internal override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            string baseDir = Environment.GetFolderPath(_folder);

            if (File != null)
            {
                builder.Append(ApplyPadding(Path.Combine(baseDir, File)));
            }
            else if (Dir != null)
            {
                builder.Append(ApplyPadding(Path.Combine(baseDir, Dir)));
            }
            else
            {
                builder.Append(ApplyPadding(baseDir));
            }
        }

        /// <summary>
        /// Determines whether the value produced by the layout renderer
        /// is fixed per current app-domain.
        /// </summary>
        /// <returns><see langword="true"/></returns>
        protected internal override bool IsAppDomainFixed()
        {
            return true;
        }
    }
}
#endif
