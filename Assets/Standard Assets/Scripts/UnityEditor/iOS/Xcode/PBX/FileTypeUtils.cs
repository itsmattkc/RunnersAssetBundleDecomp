using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
	internal class FileTypeUtils
	{
		internal class FileTypeDesc
		{
			public string name;

			public PBXFileType type;

			public bool isExplicit;

			public FileTypeDesc(string typeName, PBXFileType type)
			{
				this.name = typeName;
				this.type = type;
				this.isExplicit = false;
			}

			public FileTypeDesc(string typeName, PBXFileType type, bool isExplicit)
			{
				this.name = typeName;
				this.type = type;
				this.isExplicit = isExplicit;
			}
		}

		private static readonly Dictionary<string, FileTypeUtils.FileTypeDesc> types = new Dictionary<string, FileTypeUtils.FileTypeDesc>
		{
			{
				".a",
				new FileTypeUtils.FileTypeDesc("archive.ar", PBXFileType.Framework)
			},
			{
				".app",
				new FileTypeUtils.FileTypeDesc("wrapper.application", PBXFileType.NotBuildable, true)
			},
			{
				".appex",
				new FileTypeUtils.FileTypeDesc("wrapper.app-extension", PBXFileType.CopyFile)
			},
			{
				".s",
				new FileTypeUtils.FileTypeDesc("sourcecode.asm", PBXFileType.Source)
			},
			{
				".c",
				new FileTypeUtils.FileTypeDesc("sourcecode.c.c", PBXFileType.Source)
			},
			{
				".cc",
				new FileTypeUtils.FileTypeDesc("sourcecode.cpp.cpp", PBXFileType.Source)
			},
			{
				".cpp",
				new FileTypeUtils.FileTypeDesc("sourcecode.cpp.cpp", PBXFileType.Source)
			},
			{
				".swift",
				new FileTypeUtils.FileTypeDesc("sourcecode.swift", PBXFileType.Source)
			},
			{
				".dll",
				new FileTypeUtils.FileTypeDesc("file", PBXFileType.NotBuildable)
			},
			{
				".framework",
				new FileTypeUtils.FileTypeDesc("wrapper.framework", PBXFileType.Framework)
			},
			{
				".h",
				new FileTypeUtils.FileTypeDesc("sourcecode.c.h", PBXFileType.NotBuildable)
			},
			{
				".pch",
				new FileTypeUtils.FileTypeDesc("sourcecode.c.h", PBXFileType.NotBuildable)
			},
			{
				".icns",
				new FileTypeUtils.FileTypeDesc("image.icns", PBXFileType.Resource)
			},
			{
				".xcassets",
				new FileTypeUtils.FileTypeDesc("folder.assetcatalog", PBXFileType.Resource)
			},
			{
				".inc",
				new FileTypeUtils.FileTypeDesc("sourcecode.inc", PBXFileType.NotBuildable)
			},
			{
				".m",
				new FileTypeUtils.FileTypeDesc("sourcecode.c.objc", PBXFileType.Source)
			},
			{
				".mm",
				new FileTypeUtils.FileTypeDesc("sourcecode.cpp.objcpp", PBXFileType.Source)
			},
			{
				".nib",
				new FileTypeUtils.FileTypeDesc("wrapper.nib", PBXFileType.Resource)
			},
			{
				".plist",
				new FileTypeUtils.FileTypeDesc("text.plist.xml", PBXFileType.Resource)
			},
			{
				".png",
				new FileTypeUtils.FileTypeDesc("image.png", PBXFileType.Resource)
			},
			{
				".rtf",
				new FileTypeUtils.FileTypeDesc("text.rtf", PBXFileType.Resource)
			},
			{
				".tiff",
				new FileTypeUtils.FileTypeDesc("image.tiff", PBXFileType.Resource)
			},
			{
				".txt",
				new FileTypeUtils.FileTypeDesc("text", PBXFileType.Resource)
			},
			{
				".json",
				new FileTypeUtils.FileTypeDesc("text.json", PBXFileType.Resource)
			},
			{
				".xcodeproj",
				new FileTypeUtils.FileTypeDesc("wrapper.pb-project", PBXFileType.NotBuildable)
			},
			{
				".xib",
				new FileTypeUtils.FileTypeDesc("file.xib", PBXFileType.Resource)
			},
			{
				".strings",
				new FileTypeUtils.FileTypeDesc("text.plist.strings", PBXFileType.Resource)
			},
			{
				".storyboard",
				new FileTypeUtils.FileTypeDesc("file.storyboard", PBXFileType.Resource)
			},
			{
				".bundle",
				new FileTypeUtils.FileTypeDesc("wrapper.plug-in", PBXFileType.Resource)
			},
			{
				".dylib",
				new FileTypeUtils.FileTypeDesc("compiled.mach-o.dylib", PBXFileType.Framework)
			},
			{
				".db",
				new FileTypeUtils.FileTypeDesc("template.db", PBXFileType.Resource)
			}
		};

		private static readonly Dictionary<PBXSourceTree, string> sourceTree = new Dictionary<PBXSourceTree, string>
		{
			{
				PBXSourceTree.Absolute,
				"<absolute>"
			},
			{
				PBXSourceTree.Group,
				"<group>"
			},
			{
				PBXSourceTree.Build,
				"BUILT_PRODUCTS_DIR"
			},
			{
				PBXSourceTree.Developer,
				"DEVELOPER_DIR"
			},
			{
				PBXSourceTree.Sdk,
				"SDKROOT"
			},
			{
				PBXSourceTree.Source,
				"SOURCE_ROOT"
			}
		};

		private static readonly Dictionary<string, PBXSourceTree> stringToSourceTreeMap = new Dictionary<string, PBXSourceTree>
		{
			{
				"<absolute>",
				PBXSourceTree.Absolute
			},
			{
				"<group>",
				PBXSourceTree.Group
			},
			{
				"BUILT_PRODUCTS_DIR",
				PBXSourceTree.Build
			},
			{
				"DEVELOPER_DIR",
				PBXSourceTree.Developer
			},
			{
				"SDKROOT",
				PBXSourceTree.Sdk
			},
			{
				"SOURCE_ROOT",
				PBXSourceTree.Source
			}
		};

		public static bool IsKnownExtension(string ext)
		{
			return FileTypeUtils.types.ContainsKey(ext);
		}

		internal static bool IsFileTypeExplicit(string ext)
		{
			return FileTypeUtils.types.ContainsKey(ext) && FileTypeUtils.types[ext].isExplicit;
		}

		public static PBXFileType GetFileType(string ext)
		{
			if (FileTypeUtils.types.ContainsKey(ext))
			{
				return FileTypeUtils.types[ext].type;
			}
			return PBXFileType.NotBuildable;
		}

		public static string GetTypeName(string ext)
		{
			if (FileTypeUtils.types.ContainsKey(ext))
			{
				return FileTypeUtils.types[ext].name;
			}
			return "text";
		}

		public static bool IsBuildable(string ext)
		{
			return FileTypeUtils.types.ContainsKey(ext) && FileTypeUtils.types[ext].type != PBXFileType.NotBuildable;
		}

		internal static string SourceTreeDesc(PBXSourceTree tree)
		{
			return FileTypeUtils.sourceTree[tree];
		}

		internal static PBXSourceTree ParseSourceTree(string tree)
		{
			if (FileTypeUtils.stringToSourceTreeMap.ContainsKey(tree))
			{
				return FileTypeUtils.stringToSourceTreeMap[tree];
			}
			return PBXSourceTree.Source;
		}

		internal static List<PBXSourceTree> AllAbsoluteSourceTrees()
		{
			return new List<PBXSourceTree>
			{
				PBXSourceTree.Absolute,
				PBXSourceTree.Build,
				PBXSourceTree.Developer,
				PBXSourceTree.Sdk,
				PBXSourceTree.Source
			};
		}
	}
}
