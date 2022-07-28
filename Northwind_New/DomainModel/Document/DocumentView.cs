using System;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Document
{
    public class DocumentView
    {
        [Key]
        public Guid stream_id { get; private set; }

        public byte[] file_stream { get; private set; }

        public string name { get; private set; }

        public string path_locator { get; private set; }

        public string parent_path_locator { get; private set; }

        public string file_type { get; private set; }

        public long cached_file_size { get; private set; }

        public DateTimeOffset creation_time { get; private set; }

        public DateTimeOffset last_write_time { get; private set; }

        public DateTimeOffset last_access_time { get; private set; }

        public bool is_directory { get; private set; }

        public bool is_offline { get; private set; }

        public bool is_hidden { get; private set; }

        public bool is_readonly { get; private set; }

        public bool is_archive { get; private set; }

        public bool is_system { get; private set; }

        public bool is_temporary { get; private set; }
    }
}
