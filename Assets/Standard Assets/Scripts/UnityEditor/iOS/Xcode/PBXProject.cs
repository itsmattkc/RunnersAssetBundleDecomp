using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor.iOS.Xcode.PBX;

namespace UnityEditor.iOS.Xcode
{
	public class PBXProject
	{
		private sealed class _ReadFromStream_c__AnonStorey9
		{
			internal string prevSectionName;

			internal bool __m__9(string x)
			{
				return x == this.prevSectionName;
			}
		}

		private sealed class _RepairStructureGuidList_c__AnonStoreyA<T> where T : PBXObject, new()
		{
			internal Func<T, GUIDList> listRetrieveFunc;

			internal Dictionary<string, bool> allGuids;

			internal bool __m__A(T obj)
			{
				GUIDList gUIDList = this.listRetrieveFunc(obj);
				if (gUIDList == null)
				{
					return false;
				}
				PBXProject.RepairStructureRemoveMissingGuids(gUIDList, this.allGuids);
				return true;
			}
		}

		private sealed class _RepairStructureImpl_c__AnonStoreyB
		{
			internal Dictionary<string, bool> allGuids;

			internal bool __m__B(PBXBuildFile obj)
			{
				return obj.fileRef != null && this.allGuids.ContainsKey(obj.fileRef);
			}
		}

		private Dictionary<string, SectionBase> m_Section;

		private PBXElementDict m_RootElements;

		private PBXElementDict m_UnknownObjects;

		private string m_ObjectVersion;

		private List<string> m_SectionOrder;

		private Dictionary<string, KnownSectionBase<PBXObject>> m_UnknownSections;

		private KnownSectionBase<PBXBuildFile> buildFiles;

		private KnownSectionBase<PBXFileReference> fileRefs;

		private KnownSectionBase<PBXGroup> groups;

		private KnownSectionBase<PBXContainerItemProxy> containerItems;

		private KnownSectionBase<PBXReferenceProxy> references;

		private KnownSectionBase<PBXSourcesBuildPhase> sources;

		private KnownSectionBase<PBXFrameworksBuildPhase> frameworks;

		private KnownSectionBase<PBXResourcesBuildPhase> resources;

		private KnownSectionBase<PBXCopyFilesBuildPhase> copyFiles;

		private KnownSectionBase<PBXShellScriptBuildPhase> shellScripts;

		private KnownSectionBase<PBXNativeTarget> nativeTargets;

		private KnownSectionBase<PBXTargetDependency> targetDependencies;

		private KnownSectionBase<PBXVariantGroup> variantGroups;

		private KnownSectionBase<XCBuildConfiguration> buildConfigs;

		private KnownSectionBase<XCConfigurationList> configs;

		private PBXProjectSection project;

		private Dictionary<string, Dictionary<string, PBXBuildFile>> m_FileGuidToBuildFileMap;

		private Dictionary<string, PBXFileReference> m_ProjectPathToFileRefMap;

		private Dictionary<string, string> m_FileRefGuidToProjectPathMap;

		private Dictionary<PBXSourceTree, Dictionary<string, PBXFileReference>> m_RealPathToFileRefMap;

		private Dictionary<string, PBXGroup> m_ProjectPathToGroupMap;

		private Dictionary<string, string> m_GroupGuidToProjectPathMap;

		private Dictionary<string, PBXGroup> m_GuidToParentGroupMap;

		private static Func<PBXGroup, GUIDList> __f__am_cache1D;

		private static Func<PBXSourcesBuildPhase, GUIDList> __f__am_cache1E;

		private static Func<PBXFrameworksBuildPhase, GUIDList> __f__am_cache1F;

		private static Func<PBXResourcesBuildPhase, GUIDList> __f__am_cache20;

		private static Func<PBXCopyFilesBuildPhase, GUIDList> __f__am_cache21;

		private static Func<PBXShellScriptBuildPhase, GUIDList> __f__am_cache22;

		private static Func<PBXNativeTarget, GUIDList> __f__am_cache23;

		private static Func<PBXVariantGroup, GUIDList> __f__am_cache24;

		private static Func<XCConfigurationList, GUIDList> __f__am_cache25;

		private void BuildFilesAdd(string targetGuid, PBXBuildFile buildFile)
		{
			if (!this.m_FileGuidToBuildFileMap.ContainsKey(targetGuid))
			{
				this.m_FileGuidToBuildFileMap[targetGuid] = new Dictionary<string, PBXBuildFile>();
			}
			this.m_FileGuidToBuildFileMap[targetGuid][buildFile.fileRef] = buildFile;
			this.buildFiles.AddEntry(buildFile);
		}

		private void BuildFilesRemove(string targetGuid, string fileGuid)
		{
			PBXBuildFile buildFileForFileGuid = this.GetBuildFileForFileGuid(targetGuid, fileGuid);
			if (buildFileForFileGuid != null)
			{
				this.m_FileGuidToBuildFileMap[targetGuid].Remove(buildFileForFileGuid.fileRef);
				this.buildFiles.RemoveEntry(buildFileForFileGuid.guid);
			}
		}

		private PBXBuildFile GetBuildFileForFileGuid(string targetGuid, string fileGuid)
		{
			if (!this.m_FileGuidToBuildFileMap.ContainsKey(targetGuid))
			{
				return null;
			}
			if (!this.m_FileGuidToBuildFileMap[targetGuid].ContainsKey(fileGuid))
			{
				return null;
			}
			return this.m_FileGuidToBuildFileMap[targetGuid][fileGuid];
		}

		private void FileRefsAdd(string realPath, string projectPath, PBXGroup parent, PBXFileReference fileRef)
		{
			this.fileRefs.AddEntry(fileRef);
			this.m_ProjectPathToFileRefMap.Add(projectPath, fileRef);
			this.m_FileRefGuidToProjectPathMap.Add(fileRef.guid, projectPath);
			this.m_RealPathToFileRefMap[fileRef.tree].Add(realPath, fileRef);
			this.m_GuidToParentGroupMap.Add(fileRef.guid, parent);
		}

		private void FileRefsRemove(string guid)
		{
			PBXFileReference pBXFileReference = this.fileRefs[guid];
			this.fileRefs.RemoveEntry(guid);
			this.m_ProjectPathToFileRefMap.Remove(this.m_FileRefGuidToProjectPathMap[guid]);
			this.m_FileRefGuidToProjectPathMap.Remove(guid);
			foreach (PBXSourceTree current in FileTypeUtils.AllAbsoluteSourceTrees())
			{
				this.m_RealPathToFileRefMap[current].Remove(pBXFileReference.path);
			}
			this.m_GuidToParentGroupMap.Remove(guid);
		}

		private void GroupsAdd(string projectPath, PBXGroup parent, PBXGroup gr)
		{
			this.m_ProjectPathToGroupMap.Add(projectPath, gr);
			this.m_GroupGuidToProjectPathMap.Add(gr.guid, projectPath);
			this.m_GuidToParentGroupMap.Add(gr.guid, parent);
			this.groups.AddEntry(gr);
		}

		private void GroupsRemove(string guid)
		{
			this.m_ProjectPathToGroupMap.Remove(this.m_GroupGuidToProjectPathMap[guid]);
			this.m_GroupGuidToProjectPathMap.Remove(guid);
			this.m_GuidToParentGroupMap.Remove(guid);
			this.groups.RemoveEntry(guid);
		}

		private void RefreshBuildFilesMapForBuildFileGuidList(Dictionary<string, PBXBuildFile> mapForTarget, FileGUIDListBase list)
		{
			foreach (string current in ((IEnumerable<string>)list.files))
			{
				PBXBuildFile pBXBuildFile = this.buildFiles[current];
				mapForTarget[pBXBuildFile.fileRef] = pBXBuildFile;
			}
		}

		private void CombinePaths(string path1, PBXSourceTree tree1, string path2, PBXSourceTree tree2, out string resPath, out PBXSourceTree resTree)
		{
			if (tree2 == PBXSourceTree.Group)
			{
				resPath = Path.Combine(path1, path2);
				resTree = tree1;
				return;
			}
			resPath = path2;
			resTree = tree2;
		}

		private void RefreshMapsForGroupChildren(string projectPath, string realPath, PBXSourceTree realPathTree, PBXGroup parent)
		{
			List<string> list = new List<string>(parent.children);
			foreach (string current in list)
			{
				PBXFileReference pBXFileReference = this.fileRefs[current];
				if (pBXFileReference != null)
				{
					string text = Path.Combine(projectPath, pBXFileReference.name);
					string text2;
					PBXSourceTree pBXSourceTree;
					this.CombinePaths(realPath, realPathTree, pBXFileReference.path, pBXFileReference.tree, out text2, out pBXSourceTree);
					this.m_ProjectPathToFileRefMap.Add(text, pBXFileReference);
					this.m_FileRefGuidToProjectPathMap.Add(pBXFileReference.guid, text);
					this.m_RealPathToFileRefMap[pBXSourceTree].Add(text2, pBXFileReference);
					this.m_GuidToParentGroupMap.Add(current, parent);
				}
				else
				{
					PBXGroup pBXGroup = this.groups[current];
					if (pBXGroup != null)
					{
						string text = Path.Combine(projectPath, pBXGroup.name);
						string text2;
						PBXSourceTree pBXSourceTree;
						this.CombinePaths(realPath, realPathTree, pBXGroup.path, pBXGroup.tree, out text2, out pBXSourceTree);
						this.m_ProjectPathToGroupMap.Add(text, pBXGroup);
						this.m_GroupGuidToProjectPathMap.Add(pBXGroup.guid, text);
						this.m_GuidToParentGroupMap.Add(current, parent);
						this.RefreshMapsForGroupChildren(text, text2, pBXSourceTree, pBXGroup);
					}
				}
			}
		}

		private void RefreshAuxMaps()
		{
			foreach (KeyValuePair<string, PBXNativeTarget> current in this.nativeTargets.entries)
			{
				Dictionary<string, PBXBuildFile> dictionary = new Dictionary<string, PBXBuildFile>();
				foreach (string current2 in ((IEnumerable<string>)current.Value.phases))
				{
					if (this.frameworks.entries.ContainsKey(current2))
					{
						this.RefreshBuildFilesMapForBuildFileGuidList(dictionary, this.frameworks.entries[current2]);
					}
					if (this.resources.entries.ContainsKey(current2))
					{
						this.RefreshBuildFilesMapForBuildFileGuidList(dictionary, this.resources.entries[current2]);
					}
					if (this.sources.entries.ContainsKey(current2))
					{
						this.RefreshBuildFilesMapForBuildFileGuidList(dictionary, this.sources.entries[current2]);
					}
					if (this.copyFiles.entries.ContainsKey(current2))
					{
						this.RefreshBuildFilesMapForBuildFileGuidList(dictionary, this.copyFiles.entries[current2]);
					}
				}
				this.m_FileGuidToBuildFileMap[current.Key] = dictionary;
			}
			this.RefreshMapsForGroupChildren(string.Empty, string.Empty, PBXSourceTree.Source, this.groups[this.project.project.mainGroup]);
		}

		private void Clear()
		{
			this.buildFiles = new KnownSectionBase<PBXBuildFile>("PBXBuildFile");
			this.fileRefs = new KnownSectionBase<PBXFileReference>("PBXFileReference");
			this.groups = new KnownSectionBase<PBXGroup>("PBXGroup");
			this.containerItems = new KnownSectionBase<PBXContainerItemProxy>("PBXContainerItemProxy");
			this.references = new KnownSectionBase<PBXReferenceProxy>("PBXReferenceProxy");
			this.sources = new KnownSectionBase<PBXSourcesBuildPhase>("PBXSourcesBuildPhase");
			this.frameworks = new KnownSectionBase<PBXFrameworksBuildPhase>("PBXFrameworksBuildPhase");
			this.resources = new KnownSectionBase<PBXResourcesBuildPhase>("PBXResourcesBuildPhase");
			this.copyFiles = new KnownSectionBase<PBXCopyFilesBuildPhase>("PBXCopyFilesBuildPhase");
			this.shellScripts = new KnownSectionBase<PBXShellScriptBuildPhase>("PBXShellScriptBuildPhase");
			this.nativeTargets = new KnownSectionBase<PBXNativeTarget>("PBXNativeTarget");
			this.targetDependencies = new KnownSectionBase<PBXTargetDependency>("PBXTargetDependency");
			this.variantGroups = new KnownSectionBase<PBXVariantGroup>("PBXVariantGroup");
			this.buildConfigs = new KnownSectionBase<XCBuildConfiguration>("XCBuildConfiguration");
			this.configs = new KnownSectionBase<XCConfigurationList>("XCConfigurationList");
			this.project = new PBXProjectSection();
			this.m_UnknownSections = new Dictionary<string, KnownSectionBase<PBXObject>>();
			this.m_Section = new Dictionary<string, SectionBase>
			{
				{
					"PBXBuildFile",
					this.buildFiles
				},
				{
					"PBXFileReference",
					this.fileRefs
				},
				{
					"PBXGroup",
					this.groups
				},
				{
					"PBXContainerItemProxy",
					this.containerItems
				},
				{
					"PBXReferenceProxy",
					this.references
				},
				{
					"PBXSourcesBuildPhase",
					this.sources
				},
				{
					"PBXFrameworksBuildPhase",
					this.frameworks
				},
				{
					"PBXResourcesBuildPhase",
					this.resources
				},
				{
					"PBXCopyFilesBuildPhase",
					this.copyFiles
				},
				{
					"PBXShellScriptBuildPhase",
					this.shellScripts
				},
				{
					"PBXNativeTarget",
					this.nativeTargets
				},
				{
					"PBXTargetDependency",
					this.targetDependencies
				},
				{
					"PBXVariantGroup",
					this.variantGroups
				},
				{
					"XCBuildConfiguration",
					this.buildConfigs
				},
				{
					"XCConfigurationList",
					this.configs
				},
				{
					"PBXProject",
					this.project
				}
			};
			this.m_RootElements = new PBXElementDict();
			this.m_UnknownObjects = new PBXElementDict();
			this.m_ObjectVersion = null;
			this.m_SectionOrder = new List<string>
			{
				"PBXBuildFile",
				"PBXContainerItemProxy",
				"PBXCopyFilesBuildPhase",
				"PBXFileReference",
				"PBXFrameworksBuildPhase",
				"PBXGroup",
				"PBXNativeTarget",
				"PBXProject",
				"PBXReferenceProxy",
				"PBXResourcesBuildPhase",
				"PBXShellScriptBuildPhase",
				"PBXSourcesBuildPhase",
				"PBXTargetDependency",
				"PBXVariantGroup",
				"XCBuildConfiguration",
				"XCConfigurationList"
			};
			this.m_FileGuidToBuildFileMap = new Dictionary<string, Dictionary<string, PBXBuildFile>>();
			this.m_ProjectPathToFileRefMap = new Dictionary<string, PBXFileReference>();
			this.m_FileRefGuidToProjectPathMap = new Dictionary<string, string>();
			this.m_RealPathToFileRefMap = new Dictionary<PBXSourceTree, Dictionary<string, PBXFileReference>>();
			foreach (PBXSourceTree current in FileTypeUtils.AllAbsoluteSourceTrees())
			{
				this.m_RealPathToFileRefMap.Add(current, new Dictionary<string, PBXFileReference>());
			}
			this.m_ProjectPathToGroupMap = new Dictionary<string, PBXGroup>();
			this.m_GroupGuidToProjectPathMap = new Dictionary<string, string>();
			this.m_GuidToParentGroupMap = new Dictionary<string, PBXGroup>();
		}

		public static string GetPBXProjectPath(string buildPath)
		{
			return Path.Combine(buildPath, "Unity-iPhone/project.pbxproj");
		}

		public static string GetUnityTargetName()
		{
			return "Unity-iPhone";
		}

		public static string GetUnityTestTargetName()
		{
			return "Unity-iPhone Tests";
		}

		public string TargetGuidByName(string name)
		{
			foreach (KeyValuePair<string, PBXNativeTarget> current in this.nativeTargets.entries)
			{
				if (current.Value.name == name)
				{
					return current.Key;
				}
			}
			return null;
		}

		private FileGUIDListBase BuildSection(PBXNativeTarget target, string path)
		{
			string extension = Path.GetExtension(path);
			switch (FileTypeUtils.GetFileType(extension))
			{
			case PBXFileType.Framework:
				foreach (string current in ((IEnumerable<string>)target.phases))
				{
					if (this.frameworks.entries.ContainsKey(current))
					{
						FileGUIDListBase result = this.frameworks.entries[current];
						return result;
					}
				}
				break;
			case PBXFileType.Source:
				foreach (string current2 in ((IEnumerable<string>)target.phases))
				{
					if (this.sources.entries.ContainsKey(current2))
					{
						FileGUIDListBase result = this.sources.entries[current2];
						return result;
					}
				}
				break;
			case PBXFileType.Resource:
				foreach (string current3 in ((IEnumerable<string>)target.phases))
				{
					if (this.resources.entries.ContainsKey(current3))
					{
						FileGUIDListBase result = this.resources.entries[current3];
						return result;
					}
				}
				break;
			case PBXFileType.CopyFile:
				foreach (string current4 in ((IEnumerable<string>)target.phases))
				{
					if (this.copyFiles.entries.ContainsKey(current4))
					{
						FileGUIDListBase result = this.copyFiles.entries[current4];
						return result;
					}
				}
				break;
			}
			return null;
		}

		public static bool IsKnownExtension(string ext)
		{
			return FileTypeUtils.IsKnownExtension(ext);
		}

		public static bool IsBuildable(string ext)
		{
			return FileTypeUtils.IsBuildable(ext);
		}

		private string AddFileImpl(string path, string projectPath, PBXSourceTree tree)
		{
			path = PBXProject.FixSlashesInPath(path);
			projectPath = PBXProject.FixSlashesInPath(projectPath);
			string extension = Path.GetExtension(path);
			if (extension != Path.GetExtension(projectPath))
			{
				throw new Exception("Project and real path extensions do not match");
			}
			string text = this.FindFileGuidByProjectPath(projectPath);
			if (text == null)
			{
				text = this.FindFileGuidByRealPath(path);
			}
			if (text == null)
			{
				PBXFileReference pBXFileReference = PBXFileReference.CreateFromFile(path, this.GetFilenameFromPath(projectPath), tree);
				PBXGroup pBXGroup = this.CreateSourceGroup(this.GetDirectoryFromPath(projectPath));
				pBXGroup.children.AddGUID(pBXFileReference.guid);
				this.FileRefsAdd(path, projectPath, pBXGroup, pBXFileReference);
				text = pBXFileReference.guid;
			}
			return text;
		}

		public string AddFile(string path, string projectPath)
		{
			return this.AddFileImpl(path, projectPath, PBXSourceTree.Source);
		}

		public string AddFile(string path, string projectPath, PBXSourceTree sourceTree)
		{
			if (sourceTree == PBXSourceTree.Group)
			{
				throw new Exception("sourceTree must not be PBXSourceTree.Group");
			}
			return this.AddFileImpl(path, projectPath, sourceTree);
		}

		private void AddBuildFileImpl(string targetGuid, string fileGuid, bool weak, string compileFlags)
		{
			PBXNativeTarget target = this.nativeTargets[targetGuid];
			string extension = Path.GetExtension(this.fileRefs[fileGuid].path);
			if (FileTypeUtils.IsBuildable(extension) && this.GetBuildFileForFileGuid(targetGuid, fileGuid) == null)
			{
				PBXBuildFile pBXBuildFile = PBXBuildFile.CreateFromFile(fileGuid, weak, compileFlags);
				this.BuildFilesAdd(targetGuid, pBXBuildFile);
				this.BuildSection(target, extension).files.AddGUID(pBXBuildFile.guid);
			}
		}

		public void AddFileToBuild(string targetGuid, string fileGuid)
		{
			this.AddBuildFileImpl(targetGuid, fileGuid, false, null);
		}

		public void AddFileToBuildWithFlags(string targetGuid, string fileGuid, string compileFlags)
		{
			this.AddBuildFileImpl(targetGuid, fileGuid, false, compileFlags);
		}

		public List<string> GetCompileFlagsForFile(string targetGuid, string fileGuid)
		{
			PBXBuildFile buildFileForFileGuid = this.GetBuildFileForFileGuid(targetGuid, fileGuid);
			if (buildFileForFileGuid == null)
			{
				return null;
			}
			if (buildFileForFileGuid.compileFlags == null)
			{
				return new List<string>();
			}
			return new List<string>
			{
				buildFileForFileGuid.compileFlags
			};
		}

		public void SetCompileFlagsForFile(string targetGuid, string fileGuid, List<string> compileFlags)
		{
			PBXBuildFile buildFileForFileGuid = this.GetBuildFileForFileGuid(targetGuid, fileGuid);
			if (buildFileForFileGuid == null)
			{
				return;
			}
			buildFileForFileGuid.compileFlags = string.Join(" ", compileFlags.ToArray());
		}

		public bool ContainsFileByRealPath(string path)
		{
			return this.FindFileGuidByRealPath(path) != null;
		}

		public bool ContainsFileByRealPath(string path, PBXSourceTree sourceTree)
		{
			if (sourceTree == PBXSourceTree.Group)
			{
				throw new Exception("sourceTree must not be PBXSourceTree.Group");
			}
			return this.FindFileGuidByRealPath(path, sourceTree) != null;
		}

		public bool ContainsFileByProjectPath(string path)
		{
			return this.FindFileGuidByProjectPath(path) != null;
		}

		public bool HasFramework(string framework)
		{
			return this.ContainsFileByRealPath("System/Library/Frameworks/" + framework);
		}

		public void AddFrameworkToProject(string targetGuid, string framework, bool weak)
		{
			string fileGuid = this.AddFile("System/Library/Frameworks/" + framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
			this.AddBuildFileImpl(targetGuid, fileGuid, weak, null);
		}

		private string GetDirectoryFromPath(string path)
		{
			int num = path.LastIndexOf('/');
			if (num == -1)
			{
				return string.Empty;
			}
			return path.Substring(0, num);
		}

		private string GetFilenameFromPath(string path)
		{
			int num = path.LastIndexOf('/');
			if (num == -1)
			{
				return path;
			}
			return path.Substring(num + 1);
		}

		public string FindFileGuidByRealPath(string path, PBXSourceTree sourceTree)
		{
			if (sourceTree == PBXSourceTree.Group)
			{
				throw new Exception("sourceTree must not be PBXSourceTree.Group");
			}
			path = PBXProject.FixSlashesInPath(path);
			if (this.m_RealPathToFileRefMap[sourceTree].ContainsKey(path))
			{
				return this.m_RealPathToFileRefMap[sourceTree][path].guid;
			}
			return null;
		}

		public string FindFileGuidByRealPath(string path)
		{
			path = PBXProject.FixSlashesInPath(path);
			foreach (PBXSourceTree current in FileTypeUtils.AllAbsoluteSourceTrees())
			{
				string text = this.FindFileGuidByRealPath(path, current);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		public string FindFileGuidByProjectPath(string path)
		{
			path = PBXProject.FixSlashesInPath(path);
			if (this.m_ProjectPathToFileRefMap.ContainsKey(path))
			{
				return this.m_ProjectPathToFileRefMap[path].guid;
			}
			return null;
		}

		public void RemoveFileFromBuild(string targetGuid, string fileGuid)
		{
			PBXBuildFile buildFileForFileGuid = this.GetBuildFileForFileGuid(targetGuid, fileGuid);
			if (buildFileForFileGuid == null)
			{
				return;
			}
			this.BuildFilesRemove(targetGuid, fileGuid);
			string guid = buildFileForFileGuid.guid;
			if (guid != null)
			{
				foreach (KeyValuePair<string, PBXSourcesBuildPhase> current in this.sources.entries)
				{
					current.Value.files.RemoveGUID(guid);
				}
				foreach (KeyValuePair<string, PBXResourcesBuildPhase> current2 in this.resources.entries)
				{
					current2.Value.files.RemoveGUID(guid);
				}
				foreach (KeyValuePair<string, PBXCopyFilesBuildPhase> current3 in this.copyFiles.entries)
				{
					current3.Value.files.RemoveGUID(guid);
				}
				foreach (KeyValuePair<string, PBXFrameworksBuildPhase> current4 in this.frameworks.entries)
				{
					current4.Value.files.RemoveGUID(guid);
				}
			}
		}

		public void RemoveFile(string fileGuid)
		{
			if (fileGuid == null)
			{
				return;
			}
			PBXGroup pBXGroup = this.m_GuidToParentGroupMap[fileGuid];
			if (pBXGroup != null)
			{
				pBXGroup.children.RemoveGUID(fileGuid);
			}
			this.RemoveGroupIfEmpty(pBXGroup);
			foreach (KeyValuePair<string, PBXNativeTarget> current in this.nativeTargets.entries)
			{
				this.RemoveFileFromBuild(current.Value.guid, fileGuid);
			}
			this.FileRefsRemove(fileGuid);
		}

		private void RemoveGroupIfEmpty(PBXGroup gr)
		{
			if (gr.children.Count == 0 && gr.guid != this.project.project.mainGroup)
			{
				PBXGroup pBXGroup = this.m_GuidToParentGroupMap[gr.guid];
				pBXGroup.children.RemoveGUID(gr.guid);
				this.RemoveGroupIfEmpty(pBXGroup);
				this.GroupsRemove(gr.guid);
			}
		}

		private void RemoveGroupChildrenRecursive(PBXGroup parent)
		{
			List<string> list = new List<string>(parent.children);
			parent.children.Clear();
			foreach (string current in list)
			{
				PBXFileReference pBXFileReference = this.fileRefs[current];
				if (pBXFileReference != null)
				{
					foreach (KeyValuePair<string, PBXNativeTarget> current2 in this.nativeTargets.entries)
					{
						this.RemoveFileFromBuild(current2.Value.guid, current);
					}
					this.FileRefsRemove(current);
				}
				else
				{
					PBXGroup pBXGroup = this.groups[current];
					if (pBXGroup != null)
					{
						this.RemoveGroupChildrenRecursive(pBXGroup);
						this.GroupsRemove(parent.guid);
					}
				}
			}
		}

		internal void RemoveFilesByProjectPathRecursive(string projectPath)
		{
			PBXGroup sourceGroup = this.GetSourceGroup(projectPath);
			if (sourceGroup == null)
			{
				return;
			}
			this.RemoveGroupChildrenRecursive(sourceGroup);
			this.RemoveGroupIfEmpty(sourceGroup);
		}

		private PBXGroup GetPBXGroupChildByName(PBXGroup group, string name)
		{
			foreach (string current in ((IEnumerable<string>)group.children))
			{
				PBXGroup pBXGroup = this.groups[current];
				if (pBXGroup != null && pBXGroup.name == name)
				{
					return pBXGroup;
				}
			}
			return null;
		}

		private PBXGroup GetSourceGroup(string sourceGroup)
		{
			sourceGroup = PBXProject.FixSlashesInPath(sourceGroup);
			if (sourceGroup == null || sourceGroup == string.Empty)
			{
				return this.groups[this.project.project.mainGroup];
			}
			if (this.m_ProjectPathToGroupMap.ContainsKey(sourceGroup))
			{
				return this.m_ProjectPathToGroupMap[sourceGroup];
			}
			return null;
		}

		private PBXGroup CreateSourceGroup(string sourceGroup)
		{
			sourceGroup = PBXProject.FixSlashesInPath(sourceGroup);
			if (this.m_ProjectPathToGroupMap.ContainsKey(sourceGroup))
			{
				return this.m_ProjectPathToGroupMap[sourceGroup];
			}
			PBXGroup pBXGroup = this.groups[this.project.project.mainGroup];
			if (sourceGroup == null || sourceGroup == string.Empty)
			{
				return pBXGroup;
			}
			string[] array = sourceGroup.Trim(new char[]
			{
				'/'
			}).Split(new char[]
			{
				'/'
			});
			string text = null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (text == null)
				{
					text = text2;
				}
				else
				{
					text = text + "/" + text2;
				}
				PBXGroup pBXGroupChildByName = this.GetPBXGroupChildByName(pBXGroup, text2);
				if (pBXGroupChildByName != null)
				{
					pBXGroup = pBXGroupChildByName;
				}
				else
				{
					PBXGroup pBXGroup2 = PBXGroup.Create(text2, text2, PBXSourceTree.Group);
					pBXGroup.children.AddGUID(pBXGroup2.guid);
					this.GroupsAdd(text, pBXGroup, pBXGroup2);
					pBXGroup = pBXGroup2;
				}
			}
			return pBXGroup;
		}

		public void AddExternalProjectDependency(string path, string projectPath, PBXSourceTree sourceTree)
		{
			if (sourceTree == PBXSourceTree.Group)
			{
				throw new Exception("sourceTree must not be PBXSourceTree.Group");
			}
			path = PBXProject.FixSlashesInPath(path);
			projectPath = PBXProject.FixSlashesInPath(projectPath);
			PBXGroup pBXGroup = PBXGroup.CreateRelative("Products");
			this.groups.AddEntry(pBXGroup);
			PBXFileReference pBXFileReference = PBXFileReference.CreateFromFile(path, Path.GetFileName(projectPath), sourceTree);
			this.FileRefsAdd(path, projectPath, null, pBXFileReference);
			this.CreateSourceGroup(this.GetDirectoryFromPath(projectPath)).children.AddGUID(pBXFileReference.guid);
			this.project.project.AddReference(pBXGroup.guid, pBXFileReference.guid);
		}

		public void AddExternalLibraryDependency(string targetGuid, string filename, string remoteFileGuid, string projectPath, string remoteInfo)
		{
			PBXNativeTarget target = this.nativeTargets[targetGuid];
			filename = PBXProject.FixSlashesInPath(filename);
			projectPath = PBXProject.FixSlashesInPath(projectPath);
			string text = this.FindFileGuidByRealPath(projectPath);
			if (text == null)
			{
				throw new Exception("No such project");
			}
			string text2 = null;
			foreach (ProjectReference current in this.project.project.projectReferences)
			{
				if (current.projectRef == text)
				{
					text2 = current.group;
					break;
				}
			}
			if (text2 == null)
			{
				throw new Exception("Malformed project: no project in project references");
			}
			PBXGroup pBXGroup = this.groups[text2];
			string extension = Path.GetExtension(filename);
			if (!FileTypeUtils.IsBuildable(extension))
			{
				throw new Exception("Wrong file extension");
			}
			PBXContainerItemProxy pBXContainerItemProxy = PBXContainerItemProxy.Create(text, "2", remoteFileGuid, remoteInfo);
			this.containerItems.AddEntry(pBXContainerItemProxy);
			string typeName = FileTypeUtils.GetTypeName(extension);
			PBXReferenceProxy pBXReferenceProxy = PBXReferenceProxy.Create(filename, typeName, pBXContainerItemProxy.guid, "BUILT_PRODUCTS_DIR");
			this.references.AddEntry(pBXReferenceProxy);
			PBXBuildFile pBXBuildFile = PBXBuildFile.CreateFromFile(pBXReferenceProxy.guid, false, null);
			this.BuildFilesAdd(targetGuid, pBXBuildFile);
			this.BuildSection(target, extension).files.AddGUID(pBXBuildFile.guid);
			pBXGroup.children.AddGUID(pBXReferenceProxy.guid);
		}

		private void SetDefaultAppExtensionReleaseBuildFlags(XCBuildConfiguration config, string infoPlistPath)
		{
			config.AddProperty("ALWAYS_SEARCH_USER_PATHS", "NO");
			config.AddProperty("CLANG_CXX_LANGUAGE_STANDARD", "gnu++0x");
			config.AddProperty("CLANG_CXX_LIBRARY", "libc++");
			config.AddProperty("CLANG_ENABLE_MODULES", "YES");
			config.AddProperty("CLANG_ENABLE_OBJC_ARC", "YES");
			config.AddProperty("CLANG_WARN_BOOL_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_CONSTANT_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_DIRECT_OBJC_ISA_USAGE", "YES_ERROR");
			config.AddProperty("CLANG_WARN_EMPTY_BODY", "YES");
			config.AddProperty("CLANG_WARN_ENUM_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_INT_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_OBJC_ROOT_CLASS", "YES_ERROR");
			config.AddProperty("CLANG_WARN_UNREACHABLE_CODE", "YES");
			config.AddProperty("CLANG_WARN__DUPLICATE_METHOD_MATCH", "YES");
			config.AddProperty("COPY_PHASE_STRIP", "YES");
			config.AddProperty("ENABLE_NS_ASSERTIONS", "NO");
			config.AddProperty("ENABLE_STRICT_OBJC_MSGSEND", "YES");
			config.AddProperty("GCC_C_LANGUAGE_STANDARD", "gnu99");
			config.AddProperty("GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
			config.AddProperty("GCC_WARN_ABOUT_RETURN_TYPE", "YES_ERROR");
			config.AddProperty("GCC_WARN_UNDECLARED_SELECTOR", "YES");
			config.AddProperty("GCC_WARN_UNINITIALIZED_AUTOS", "YES_AGGRESSIVE");
			config.AddProperty("GCC_WARN_UNUSED_FUNCTION", "YES");
			config.AddProperty("INFOPLIST_FILE", infoPlistPath);
			config.AddProperty("IPHONEOS_DEPLOYMENT_TARGET", "8.0");
			config.AddProperty("LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks @executable_path/../../Frameworks");
			config.AddProperty("MTL_ENABLE_DEBUG_INFO", "NO");
			config.AddProperty("PRODUCT_NAME", "$(TARGET_NAME)");
			config.AddProperty("SKIP_INSTALL", "YES");
			config.AddProperty("VALIDATE_PRODUCT", "YES");
		}

		private void SetDefaultAppExtensionDebugBuildFlags(XCBuildConfiguration config, string infoPlistPath)
		{
			config.AddProperty("ALWAYS_SEARCH_USER_PATHS", "NO");
			config.AddProperty("CLANG_CXX_LANGUAGE_STANDARD", "gnu++0x");
			config.AddProperty("CLANG_CXX_LIBRARY", "libc++");
			config.AddProperty("CLANG_ENABLE_MODULES", "YES");
			config.AddProperty("CLANG_ENABLE_OBJC_ARC", "YES");
			config.AddProperty("CLANG_WARN_BOOL_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_CONSTANT_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_DIRECT_OBJC_ISA_USAGE", "YES_ERROR");
			config.AddProperty("CLANG_WARN_EMPTY_BODY", "YES");
			config.AddProperty("CLANG_WARN_ENUM_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_INT_CONVERSION", "YES");
			config.AddProperty("CLANG_WARN_OBJC_ROOT_CLASS", "YES_ERROR");
			config.AddProperty("CLANG_WARN_UNREACHABLE_CODE", "YES");
			config.AddProperty("CLANG_WARN__DUPLICATE_METHOD_MATCH", "YES");
			config.AddProperty("COPY_PHASE_STRIP", "NO");
			config.AddProperty("ENABLE_STRICT_OBJC_MSGSEND", "YES");
			config.AddProperty("GCC_C_LANGUAGE_STANDARD", "gnu99");
			config.AddProperty("GCC_DYNAMIC_NO_PIC", "NO");
			config.AddProperty("GCC_OPTIMIZATION_LEVEL", "0");
			config.AddProperty("GCC_PREPROCESSOR_DEFINITIONS", "DEBUG=1");
			config.AddProperty("GCC_PREPROCESSOR_DEFINITIONS", "$(inherited)");
			config.AddProperty("GCC_SYMBOLS_PRIVATE_EXTERN", "NO");
			config.AddProperty("GCC_WARN_64_TO_32_BIT_CONVERSION", "YES");
			config.AddProperty("GCC_WARN_ABOUT_RETURN_TYPE", "YES_ERROR");
			config.AddProperty("GCC_WARN_UNDECLARED_SELECTOR", "YES");
			config.AddProperty("GCC_WARN_UNINITIALIZED_AUTOS", "YES_AGGRESSIVE");
			config.AddProperty("GCC_WARN_UNUSED_FUNCTION", "YES");
			config.AddProperty("INFOPLIST_FILE", infoPlistPath);
			config.AddProperty("IPHONEOS_DEPLOYMENT_TARGET", "8.0");
			config.AddProperty("LD_RUNPATH_SEARCH_PATHS", "$(inherited)");
			config.AddProperty("LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
			config.AddProperty("LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks");
			config.AddProperty("MTL_ENABLE_DEBUG_INFO", "YES");
			config.AddProperty("ONLY_ACTIVE_ARCH", "YES");
			config.AddProperty("PRODUCT_NAME", "$(TARGET_NAME)");
			config.AddProperty("SKIP_INSTALL", "YES");
		}

		internal string AddAppExtension(string mainTarget, string name, string infoPlistPath)
		{
			string str = ".appex";
			string text = name + str;
			PBXFileReference pBXFileReference = PBXFileReference.CreateFromFile("Products/" + text, "Products/" + text, PBXSourceTree.Group);
			XCBuildConfiguration xCBuildConfiguration = XCBuildConfiguration.Create("Release");
			this.buildConfigs.AddEntry(xCBuildConfiguration);
			this.SetDefaultAppExtensionReleaseBuildFlags(xCBuildConfiguration, infoPlistPath);
			XCBuildConfiguration xCBuildConfiguration2 = XCBuildConfiguration.Create("Debug");
			this.buildConfigs.AddEntry(xCBuildConfiguration2);
			this.SetDefaultAppExtensionDebugBuildFlags(xCBuildConfiguration2, infoPlistPath);
			XCConfigurationList xCConfigurationList = XCConfigurationList.Create();
			this.configs.AddEntry(xCConfigurationList);
			xCConfigurationList.buildConfigs.AddGUID(xCBuildConfiguration.guid);
			xCConfigurationList.buildConfigs.AddGUID(xCBuildConfiguration2.guid);
			PBXNativeTarget pBXNativeTarget = PBXNativeTarget.Create(name, pBXFileReference.guid, "com.apple.product-type.app-extension", xCConfigurationList.guid);
			this.nativeTargets.AddEntry(pBXNativeTarget);
			this.project.project.targets.Add(pBXNativeTarget.guid);
			PBXSourcesBuildPhase pBXSourcesBuildPhase = PBXSourcesBuildPhase.Create();
			this.sources.AddEntry(pBXSourcesBuildPhase);
			pBXNativeTarget.phases.AddGUID(pBXSourcesBuildPhase.guid);
			PBXResourcesBuildPhase pBXResourcesBuildPhase = PBXResourcesBuildPhase.Create();
			this.resources.AddEntry(pBXResourcesBuildPhase);
			pBXNativeTarget.phases.AddGUID(pBXResourcesBuildPhase.guid);
			PBXFrameworksBuildPhase pBXFrameworksBuildPhase = PBXFrameworksBuildPhase.Create();
			this.frameworks.AddEntry(pBXFrameworksBuildPhase);
			pBXNativeTarget.phases.AddGUID(pBXFrameworksBuildPhase.guid);
			PBXCopyFilesBuildPhase pBXCopyFilesBuildPhase = PBXCopyFilesBuildPhase.Create("Embed App Extensions", "13");
			this.copyFiles.AddEntry(pBXCopyFilesBuildPhase);
			this.nativeTargets[mainTarget].phases.AddGUID(pBXCopyFilesBuildPhase.guid);
			PBXContainerItemProxy pBXContainerItemProxy = PBXContainerItemProxy.Create(this.project.project.guid, "1", pBXNativeTarget.guid, name);
			this.containerItems.AddEntry(pBXContainerItemProxy);
			PBXTargetDependency pBXTargetDependency = PBXTargetDependency.Create(pBXNativeTarget.guid, pBXContainerItemProxy.guid);
			this.targetDependencies.AddEntry(pBXTargetDependency);
			this.nativeTargets[mainTarget].dependencies.AddGUID(pBXTargetDependency.guid);
			this.AddFile(text, "Products/" + text, PBXSourceTree.Build);
			PBXBuildFile pBXBuildFile = PBXBuildFile.CreateFromFile(this.FindFileGuidByProjectPath("Products/" + text), false, string.Empty);
			this.BuildFilesAdd(mainTarget, pBXBuildFile);
			pBXCopyFilesBuildPhase.files.AddGUID(pBXBuildFile.guid);
			this.AddFile(infoPlistPath, name + "/Supporting Files/Info.plist", PBXSourceTree.Group);
			return pBXNativeTarget.guid;
		}

		public string BuildConfigByName(string targetGuid, string name)
		{
			PBXNativeTarget pBXNativeTarget = this.nativeTargets[targetGuid];
			foreach (string current in ((IEnumerable<string>)this.configs[pBXNativeTarget.buildConfigList].buildConfigs))
			{
				XCBuildConfiguration xCBuildConfiguration = this.buildConfigs[current];
				if (xCBuildConfiguration != null && xCBuildConfiguration.name == name)
				{
					return xCBuildConfiguration.guid;
				}
			}
			return null;
		}

		public void AddBuildProperty(string targetGuid, string name, string value)
		{
			PBXNativeTarget pBXNativeTarget = this.nativeTargets[targetGuid];
			foreach (string current in ((IEnumerable<string>)this.configs[pBXNativeTarget.buildConfigList].buildConfigs))
			{
				this.buildConfigs[current].AddProperty(name, value);
			}
		}

		public void AddBuildProperty(string[] targetGuids, string name, string value)
		{
			for (int i = 0; i < targetGuids.Length; i++)
			{
				string targetGuid = targetGuids[i];
				this.AddBuildProperty(targetGuid, name, value);
			}
		}

		public void AddBuildPropertyForConfig(string configGuid, string name, string value)
		{
			this.buildConfigs[configGuid].AddProperty(name, value);
		}

		public void AddBuildPropertyForConfig(string[] configGuids, string name, string value)
		{
			for (int i = 0; i < configGuids.Length; i++)
			{
				string configGuid = configGuids[i];
				this.AddBuildPropertyForConfig(configGuid, name, value);
			}
		}

		public void SetBuildProperty(string targetGuid, string name, string value)
		{
			PBXNativeTarget pBXNativeTarget = this.nativeTargets[targetGuid];
			foreach (string current in ((IEnumerable<string>)this.configs[pBXNativeTarget.buildConfigList].buildConfigs))
			{
				this.buildConfigs[current].SetProperty(name, value);
			}
		}

		public void SetBuildProperty(string[] targetGuids, string name, string value)
		{
			for (int i = 0; i < targetGuids.Length; i++)
			{
				string targetGuid = targetGuids[i];
				this.SetBuildProperty(targetGuid, name, value);
			}
		}

		public void SetBuildPropertyForConfig(string configGuid, string name, string value)
		{
			this.buildConfigs[configGuid].SetProperty(name, value);
		}

		public void SetBuildPropertyForConfig(string[] configGuids, string name, string value)
		{
			for (int i = 0; i < configGuids.Length; i++)
			{
				string configGuid = configGuids[i];
				this.SetBuildPropertyForConfig(configGuid, name, value);
			}
		}

		public void UpdateBuildProperty(string targetGuid, string name, string[] addValues, string[] removeValues)
		{
			PBXNativeTarget pBXNativeTarget = this.nativeTargets[targetGuid];
			foreach (string current in ((IEnumerable<string>)this.configs[pBXNativeTarget.buildConfigList].buildConfigs))
			{
				this.buildConfigs[current].UpdateProperties(name, addValues, removeValues);
			}
		}

		public void UpdateBuildProperty(string[] targetGuids, string name, string[] addValues, string[] removeValues)
		{
			for (int i = 0; i < targetGuids.Length; i++)
			{
				string targetGuid = targetGuids[i];
				this.UpdateBuildProperty(targetGuid, name, addValues, removeValues);
			}
		}

		public void UpdateBuildPropertyForConfig(string configGuid, string name, string[] addValues, string[] removeValues)
		{
			this.buildConfigs[configGuid].UpdateProperties(name, addValues, removeValues);
		}

		public void UpdateBuildPropertyForConfig(string[] configGuids, string name, string[] addValues, string[] removeValues)
		{
			for (int i = 0; i < configGuids.Length; i++)
			{
				string targetGuid = configGuids[i];
				this.UpdateBuildProperty(targetGuid, name, addValues, removeValues);
			}
		}

		private static string FixSlashesInPath(string path)
		{
			if (path == null)
			{
				return null;
			}
			return path.Replace('\\', '/');
		}

		private void BuildCommentMapForBuildFiles(GUIDToCommentMap comments, List<string> guids, string sectName)
		{
			foreach (string current in guids)
			{
				PBXBuildFile pBXBuildFile = this.buildFiles[current];
				if (pBXBuildFile != null)
				{
					PBXFileReference pBXFileReference = this.fileRefs[pBXBuildFile.fileRef];
					if (pBXFileReference != null)
					{
						comments.Add(current, string.Format("{0} in {1}", pBXFileReference.name, sectName));
					}
					else
					{
						PBXReferenceProxy pBXReferenceProxy = this.references[pBXBuildFile.fileRef];
						if (pBXReferenceProxy != null)
						{
							comments.Add(current, string.Format("{0} in {1}", pBXReferenceProxy.path, sectName));
						}
					}
				}
			}
		}

		private GUIDToCommentMap BuildCommentMap()
		{
			GUIDToCommentMap gUIDToCommentMap = new GUIDToCommentMap();
			foreach (PBXGroup current in this.groups.entries.Values)
			{
				gUIDToCommentMap.Add(current.guid, current.name);
			}
			foreach (PBXContainerItemProxy current2 in this.containerItems.entries.Values)
			{
				gUIDToCommentMap.Add(current2.guid, "PBXContainerItemProxy");
			}
			foreach (PBXReferenceProxy current3 in this.references.entries.Values)
			{
				gUIDToCommentMap.Add(current3.guid, current3.path);
			}
			foreach (PBXSourcesBuildPhase current4 in this.sources.entries.Values)
			{
				gUIDToCommentMap.Add(current4.guid, "Sources");
				this.BuildCommentMapForBuildFiles(gUIDToCommentMap, current4.files, "Sources");
			}
			foreach (PBXResourcesBuildPhase current5 in this.resources.entries.Values)
			{
				gUIDToCommentMap.Add(current5.guid, "Resources");
				this.BuildCommentMapForBuildFiles(gUIDToCommentMap, current5.files, "Resources");
			}
			foreach (PBXFrameworksBuildPhase current6 in this.frameworks.entries.Values)
			{
				gUIDToCommentMap.Add(current6.guid, "Frameworks");
				this.BuildCommentMapForBuildFiles(gUIDToCommentMap, current6.files, "Frameworks");
			}
			foreach (PBXCopyFilesBuildPhase current7 in this.copyFiles.entries.Values)
			{
				string text = current7.name;
				if (text == null)
				{
					text = "CopyFiles";
				}
				gUIDToCommentMap.Add(current7.guid, text);
				this.BuildCommentMapForBuildFiles(gUIDToCommentMap, current7.files, text);
			}
			foreach (PBXShellScriptBuildPhase current8 in this.shellScripts.entries.Values)
			{
				gUIDToCommentMap.Add(current8.guid, "ShellScript");
			}
			foreach (PBXTargetDependency current9 in this.targetDependencies.entries.Values)
			{
				gUIDToCommentMap.Add(current9.guid, "PBXTargetDependency");
			}
			foreach (PBXNativeTarget current10 in this.nativeTargets.entries.Values)
			{
				gUIDToCommentMap.Add(current10.guid, current10.name);
				gUIDToCommentMap.Add(current10.buildConfigList, string.Format("Build configuration list for PBXNativeTarget \"{0}\"", current10.name));
			}
			foreach (PBXVariantGroup current11 in this.variantGroups.entries.Values)
			{
				gUIDToCommentMap.Add(current11.guid, current11.name);
			}
			foreach (XCBuildConfiguration current12 in this.buildConfigs.entries.Values)
			{
				gUIDToCommentMap.Add(current12.guid, current12.name);
			}
			foreach (PBXProjectObject current13 in this.project.entries.Values)
			{
				gUIDToCommentMap.Add(current13.guid, "Project object");
				gUIDToCommentMap.Add(current13.buildConfigList, "Build configuration list for PBXProject \"Unity-iPhone\"");
			}
			foreach (PBXFileReference current14 in this.fileRefs.entries.Values)
			{
				gUIDToCommentMap.Add(current14.guid, current14.name);
			}
			if (this.m_RootElements.Contains("rootObject") && this.m_RootElements["rootObject"] is PBXElementString)
			{
				gUIDToCommentMap.Add(this.m_RootElements["rootObject"].AsString(), "Project object");
			}
			return gUIDToCommentMap;
		}

		public void ReadFromFile(string path)
		{
			this.ReadFromString(File.ReadAllText(path));
		}

		public void ReadFromString(string src)
		{
			TextReader sr = new StringReader(src);
			this.ReadFromStream(sr);
		}

		private static PBXElementDict ParseContent(string content)
		{
			TokenList tokens = Lexer.Tokenize(content);
			Parser parser = new Parser(tokens);
			TreeAST ast = parser.ParseTree();
			return Serializer.ParseTreeAST(ast, tokens, content);
		}

		public void ReadFromStream(TextReader sr)
		{
			this.Clear();
			this.m_RootElements = PBXProject.ParseContent(sr.ReadToEnd());
			if (!this.m_RootElements.Contains("objects"))
			{
				throw new Exception("Invalid PBX project file: no objects element");
			}
			PBXElementDict pBXElementDict = this.m_RootElements["objects"].AsDict();
			this.m_RootElements.Remove("objects");
			this.m_RootElements.SetString("objects", "OBJMARKER");
			if (this.m_RootElements.Contains("objectVersion"))
			{
				this.m_ObjectVersion = this.m_RootElements["objectVersion"].AsString();
				this.m_RootElements.Remove("objectVersion");
			}
			List<string> list = new List<string>();
			string prevSectionName = null;
			foreach (KeyValuePair<string, PBXElement> current in pBXElementDict.values)
			{
				list.Add(current.Key);
				PBXElement value = current.Value;
				if (!(value is PBXElementDict) || !value.AsDict().Contains("isa"))
				{
					this.m_UnknownObjects.values.Add(current.Key, value);
				}
				else
				{
					PBXElementDict pBXElementDict2 = value.AsDict();
					string text = pBXElementDict2["isa"].AsString();
					if (this.m_Section.ContainsKey(text))
					{
						SectionBase sectionBase = this.m_Section[text];
						sectionBase.AddObject(current.Key, pBXElementDict2);
					}
					else
					{
						KnownSectionBase<PBXObject> knownSectionBase;
						if (this.m_UnknownSections.ContainsKey(text))
						{
							knownSectionBase = this.m_UnknownSections[text];
						}
						else
						{
							knownSectionBase = new KnownSectionBase<PBXObject>(text);
							this.m_UnknownSections.Add(text, knownSectionBase);
						}
						knownSectionBase.AddObject(current.Key, pBXElementDict2);
						if (!this.m_SectionOrder.Contains(text))
						{
							int num = 0;
							if (prevSectionName != null)
							{
								num = this.m_SectionOrder.FindIndex((string x) => x == prevSectionName);
								num++;
							}
							this.m_SectionOrder.Insert(num, text);
						}
					}
					prevSectionName = text;
				}
			}
			this.RepairStructure(list);
			this.RefreshAuxMaps();
		}

		public void WriteToFile(string path)
		{
			File.WriteAllText(path, this.WriteToString());
		}

		public void WriteToStream(TextWriter sw)
		{
			sw.Write(this.WriteToString());
		}

		public string WriteToString()
		{
			GUIDToCommentMap comments = this.BuildCommentMap();
			PropertyCommentChecker checker = new PropertyCommentChecker();
			GUIDToCommentMap comments2 = new GUIDToCommentMap();
			StringBuilder stringBuilder = new StringBuilder();
			if (this.m_ObjectVersion != null)
			{
				stringBuilder.AppendFormat("objectVersion = {0};\n\t", this.m_ObjectVersion);
			}
			stringBuilder.Append("objects = {");
			foreach (string current in this.m_SectionOrder)
			{
				if (this.m_Section.ContainsKey(current))
				{
					this.m_Section[current].WriteSection(stringBuilder, comments);
				}
				else if (this.m_UnknownSections.ContainsKey(current))
				{
					this.m_UnknownSections[current].WriteSection(stringBuilder, comments);
				}
			}
			foreach (KeyValuePair<string, PBXElement> current2 in this.m_UnknownObjects.values)
			{
				Serializer.WriteDictKeyValue(stringBuilder, current2.Key, current2.Value, 2, false, checker, comments2);
			}
			stringBuilder.Append("\n\t};");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendLine("// !$*UTF8*$!");
			Serializer.WriteDict(stringBuilder2, this.m_RootElements, 0, false, new PropertyCommentChecker(new string[]
			{
				"rootObject/*"
			}), comments);
			stringBuilder2.AppendLine();
			string text = stringBuilder2.ToString();
			return text.Replace("objects = OBJMARKER;", stringBuilder.ToString());
		}

		private void RepairStructure(List<string> allGuids)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (string current in allGuids)
			{
				dictionary.Add(current, false);
			}
			while (!this.RepairStructureImpl(dictionary))
			{
			}
		}

		private static void RepairStructureRemoveMissingGuids(GUIDList guidList, Dictionary<string, bool> allGuids)
		{
			List<string> list = null;
			foreach (string current in ((IEnumerable<string>)guidList))
			{
				if (!allGuids.ContainsKey(current))
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(current);
				}
			}
			if (list != null)
			{
				foreach (string current2 in list)
				{
					guidList.RemoveGUID(current2);
				}
			}
		}

		private static void RepairStructureAnyType<T>(KnownSectionBase<T> section, Func<T, bool> checker, Dictionary<string, bool> allGuids, ref bool ok) where T : PBXObject, new()
		{
			List<string> list = null;
			foreach (KeyValuePair<string, T> current in section.entries)
			{
				if (!checker(current.Value))
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(current.Key);
				}
			}
			if (list != null)
			{
				ok = false;
				foreach (string current2 in list)
				{
					section.RemoveEntry(current2);
					allGuids.Remove(current2);
				}
			}
		}

		private static void RepairStructureGuidList<T>(KnownSectionBase<T> section, Func<T, GUIDList> listRetrieveFunc, Dictionary<string, bool> allGuids, ref bool ok) where T : PBXObject, new()
		{
			Func<T, bool> checker = delegate(T obj)
			{
				GUIDList gUIDList = listRetrieveFunc(obj);
				if (gUIDList == null)
				{
					return false;
				}
				PBXProject.RepairStructureRemoveMissingGuids(gUIDList, allGuids);
				return true;
			};
			PBXProject.RepairStructureAnyType<T>(section, checker, allGuids, ref ok);
		}

		private bool RepairStructureImpl(Dictionary<string, bool> allGuids)
		{
			bool result = true;
			Func<PBXBuildFile, bool> checker = (PBXBuildFile obj) => obj.fileRef != null && allGuids.ContainsKey(obj.fileRef);
			PBXProject.RepairStructureAnyType<PBXBuildFile>(this.buildFiles, checker, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXGroup>(this.groups, (PBXGroup o) => o.children, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXSourcesBuildPhase>(this.sources, (PBXSourcesBuildPhase o) => o.files, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXFrameworksBuildPhase>(this.frameworks, (PBXFrameworksBuildPhase o) => o.files, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXResourcesBuildPhase>(this.resources, (PBXResourcesBuildPhase o) => o.files, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXCopyFilesBuildPhase>(this.copyFiles, (PBXCopyFilesBuildPhase o) => o.files, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXShellScriptBuildPhase>(this.shellScripts, (PBXShellScriptBuildPhase o) => o.files, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXNativeTarget>(this.nativeTargets, (PBXNativeTarget o) => o.phases, allGuids, ref result);
			PBXProject.RepairStructureGuidList<PBXVariantGroup>(this.variantGroups, (PBXVariantGroup o) => o.children, allGuids, ref result);
			PBXProject.RepairStructureGuidList<XCConfigurationList>(this.configs, (XCConfigurationList o) => o.buildConfigs, allGuids, ref result);
			return result;
		}
	}
}
