namespace ExtensionInstaller
{
   public interface IExtensionInstaller
   {
      string Profile { get; }
      void Install(string zipFilePath, string extensionDirName, bool showOnToolbar);
   }
}
