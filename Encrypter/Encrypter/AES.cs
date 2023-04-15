using System;
using System.IO;
using System.Security.Cryptography;

// Token: 0x02000002 RID: 2
internal class AES
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Main(string[] args)
	{
		bool flag = args.Length != 1;
		if (flag)
		{
			Console.WriteLine("You must provide the name of a file to encrypt.");
		}
		else
		{
			FileInfo fileInfo = new FileInfo(args[0]);
			string destFile = Path.ChangeExtension(fileInfo.Name, ".enc");
			long value = DateTimeOffset.Now.ToUnixTimeSeconds();
			Random random = new Random(Convert.ToInt32(value));
			byte[] array = new byte[16];
			random.NextBytes(array);
			byte[] array2 = new byte[32];
			random.NextBytes(array2);
			byte[] array3 = AES.EncryptFile(fileInfo.Name, destFile, array2, array);
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020E8 File Offset: 0x000002E8
	private static byte[] EncryptFile(string sourceFile, string destFile, byte[] Key, byte[] IV)
	{
		using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
		{
			using (FileStream fileStream = new FileStream(destFile, FileMode.Create))
			{
				using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(Key, IV))
				{
					using (CryptoStream cryptoStream = new CryptoStream(fileStream, cryptoTransform, CryptoStreamMode.Write))
					{
						using (FileStream fileStream2 = new FileStream(sourceFile, FileMode.Open))
						{
							byte[] array = new byte[1024];
							int count;
							while ((count = fileStream2.Read(array, 0, array.Length)) != 0)
							{
								cryptoStream.Write(array, 0, count);
							}
						}
					}
				}
			}
		}
		return null;
	}
}
