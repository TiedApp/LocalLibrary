namespace TiedAppLocal
{
    public static class StaticFunctions
    {
        public static void CreateFileByte(string filePath, byte[] byteToWrite)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                try
                {
                    using (BinaryWriter bw = new(stream))
                    {
                        bw.Write(byteToWrite);
                        bw.Dispose();
                        bw.Close();
                    }
                    stream.Dispose();
                    stream.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        public static string DeleteFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return "Path empty";
                if (File.Exists(path))
                    File.Delete(path);

                if (File.Exists(path))
                    return "File not deleted";

                return string.Empty;
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public static byte[] ReadFileByte(string filePath)
        {
            byte[] byteToReturn = null;
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    using (BinaryReader br = new(stream))
                    {
                        byteToReturn = br.ReadBytes((int)stream.Length);
                        br.Dispose();
                        br.Close();
                    }
                    stream.Dispose();
                    stream.Close();
                }
            }
            catch (IOException)
            {
            }
            return byteToReturn;
        }
    }
}
