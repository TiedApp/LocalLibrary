using System;

namespace GlobalShared
{
    public class LocalFileInfo
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string Activity { get; set; }
        public string Reference { get; set; }
        public string Foldername { get; set; }
        public string FileFolder { get; set; }
        public string Filename { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public byte[] FileByte { get; set; }
        public string RequestCodeJson { get; set; }
        public bool DefautlFileProcess { get; set; }
        public bool ActionRequestProcess { get; set; }
        public bool InternalComProcess { get; set; }
        public string RequestReference { get; set; }
        public string ComReference { get; set; }
    }
}
