using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PavoStudio.MAE
{
    [System.Serializable]
    class PSMuscleHandle
    {
        public PSTabMuscle tabMuscle;
        public PSTabOption tabOption;
        public GameObject target;
        public Avatar avatar;
        public Vector2[] muscleMinMaxValues;
        public BoneInfo[] boneInfos;
        public int selected;
        public int rootSelected;
        public int focusId = -1;
        public const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        public MethodInfo getAxisLength;
        public MethodInfo getPreRotation;
        public MethodInfo getPostRotation;
        public MethodInfo getZYRoll;
        public MethodInfo getLimitSign;
        public MethodInfo getZYPostQ;

        [System.Serializable]
        public class BoneInfo
        {
            public Transform bone;
            public string humanBoneName;
            public int boneId;
            public int muscleXId;
            public int muscleYId;
            public int muscleZId;
        }

        public void setTabs(PSTabMuscle tabMuscle, PSTabOption tabOption)
        {
            this.tabMuscle = tabMuscle;
            this.tabOption = tabOption;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
            selected = -1;
            if (target == null)
                return;

            Animator animator = target.GetComponent(typeof(Animator)) as Animator;
            if (animator == null)
                return;

            avatar = animator.avatar;
            if (avatar == null || !avatar.isValid || !avatar.isHuman)
                return;

            muscleMinMaxValues = PSMuscleDefine.GetMuscleMinMaxValues();

            Array boneArray = Enum.GetValues(typeof(HumanBodyBones));
            Array.Sort(boneArray);

            boneInfos = new BoneInfo[HumanTrait.BoneCount];

            for (int i = 0; i < HumanTrait.BoneCount; i++)
            {
                BoneInfo info = new BoneInfo();
                info.bone = animator.GetBoneTransform((HumanBodyBones)boneArray.GetValue(i));
                info.humanBoneName = HumanTrait.BoneName[i];
                info.boneId = i;

                info.muscleXId = HumanTrait.MuscleFromBone(info.boneId, 0);
                info.muscleYId = HumanTrait.MuscleFromBone(info.boneId, 1);
                info.muscleZId = HumanTrait.MuscleFromBone(info.boneId, 2);
                if (info.muscleXId != -1 || info.muscleYId != -1 || info.muscleZId != -1)
                    boneInfos[i] = info;
            }

            // get avatar methods
            getAxisLength = avatar.GetType().GetMethod("GetAxisLength", flags);
            getPreRotation = avatar.GetType().GetMethod("GetPreRotation", flags);
            getPostRotation = avatar.GetType().GetMethod("GetPostRotation", flags);
            getZYRoll = avatar.GetType().GetMethod("GetZYRoll", flags);
            getLimitSign = avatar.GetType().GetMethod("GetLimitSign", flags);
            getZYPostQ = avatar.GetType().GetMethod("GetZYPostQ", flags);

        }

        public void ShowHandles()
        {
            if (target == null || !tabOption.showWindowTool)
                return;

            for (int i = 0, len = PSMuscleDefine.TQGroup.Length; i < len; i++)
            {
                int tid = i * 3;
                Vector3 rootT = new Vector3(tabMuscle.TValue[tid], tabMuscle.TValue[tid + 1], tabMuscle.TValue[tid + 2]);
                DrawCap(1000 + i, rootT, tabOption.TQSize, tabOption.TQColor, PSTabOption.caps[tabOption.TQShape]);
            }

            if (selected > -1)
            {
                Vector2 v = EditorGUIUtility.PixelsToPoints(new Vector2(Screen.width, Screen.height));
                Rect rect = new Rect(v.x - 220, v.y - 180, 200, 100);

                Handles.BeginGUI();
                if (selected >= 1000)
                {
                    int gid = selected - 1000;
                    if (gid < 0 || gid >= PSMuscleDefine.TQGroup.Length)
                        return;
                    GUILayout.Window(1, rect, RootGUI, PSMuscleDefine.TQGroup[gid]);
                }
                else
                    GUILayout.Window(2, rect, MuscleGUI, boneInfos[selected].humanBoneName);
                Handles.EndGUI();
            }

            for (int i = 0; i < boneInfos.Length; i++)
            {
                BoneInfo rot = boneInfos[i];
                if (rot == null || rot.bone == null)
                    continue;

                if (tabOption.showBoneNames && (!tabOption.showSelectedNameOnly || selected == i))
                    DrawLabel(rot.bone.position, rot.humanBoneName);

                DrawCap(i, rot.bone.position, tabOption.boneSize, tabOption.boneColor, PSTabOption.caps[tabOption.boneShape]);
            }

            if (selected >= 1000)
                DrawRootHandles();
            else if (selected > -1)
                DrawMuscleHandle();

            if (tabOption.showSkeleton)
                DrawSkeleton(target.transform, true);

        }

        private void DrawCap(int i, Vector3 position, float boneSize, Color color, Handles.CapFunction cap)
        {
            if (selected == i)
                Handles.color = tabOption.colorSelected;
            else
                Handles.color = color;

            PSHandles.DragHandleResult dhResult = PSHandles.DragHandleResult.none;
            PSHandles.DragHandle(position, boneSize, cap, Handles.color, out dhResult);

            switch (dhResult)
            {
                case PSHandles.DragHandleResult.LMBClick:
                    selected = i;
                    break;
            }
        }

        private void DrawLabel(Vector3 position, string text)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = tabOption.boneNameColor;
            Handles.Label(position, text, style);
        }

        void RootGUI(int id)
        {
            int gid = selected - 1000;
            if (gid < 0 || gid >= PSMuscleDefine.TQGroup.Length)
                return;

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(rootSelected == 0, "Position", "Button"))
                rootSelected = 0;
            if (GUILayout.Toggle(rootSelected == 1, "Rotation", "Button"))
                rootSelected = 1;
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (rootSelected == 0)
            {
                int tid = gid * 3;
                GUILayout.Label("position.x: " + tabMuscle.TValue[tid]);
                GUILayout.Label("position.y: " + tabMuscle.TValue[tid + 1]);
                GUILayout.Label("position.z: " + tabMuscle.TValue[tid + 2]);
            }
            else if (rootSelected == 1)
            {
                int qid = gid * 4;
                Quaternion rootQ = new Quaternion(tabMuscle.QValue[qid], tabMuscle.QValue[qid + 1], tabMuscle.QValue[qid + 2], tabMuscle.QValue[qid + 3] == 0 ? 1 : tabMuscle.QValue[qid + 3]);
                GUILayout.Label("rotation.x: " + rootQ.eulerAngles.x);
                GUILayout.Label("rotation.y: " + rootQ.eulerAngles.y);
                GUILayout.Label("rotation.z: " + rootQ.eulerAngles.z);
            }
        }

        private void DrawSkeleton(Transform trans, bool isHumanBone)
        {
            for (int i = 0, len = trans.childCount; i < len; i++)
            {
                Transform child = trans.GetChild(i);

                bool found = false;
                for (int j = 0, len2 = boneInfos.Length; j < len2; j++)
                {
                    BoneInfo bi = boneInfos[j];
                    if (bi != null && bi.bone == child)
                    {
                        found = true;
                        break;
                    }
                }

                if (isHumanBone && found)
                {
                    Handles.color = tabOption.humanSkeletonColor;
                    //Handles.DrawLine (trans.position, child.position);
                    Handles.DrawAAPolyLine(tabOption.skeletonWidth, new Vector3[] {
                        trans.position,
                        child.position
                    });
                }
                else if (!tabOption.showHumanSkeletonOnly)
                {
                    Handles.color = tabOption.skeletonColor;
                    //Handles.DrawLine (trans.position, child.position);
                    Handles.DrawAAPolyLine(tabOption.skeletonWidth, new Vector3[] {
                        trans.position,
                        child.position
                    });
                }

                DrawSkeleton(child, found);
            }
        }

        private void DrawRootHandles()
        {
            int gid = selected - 1000;
            if (gid < 0 || gid >= PSMuscleDefine.TQGroup.Length)
                return;

            int tid = gid * 3;
            Vector3 rootT = new Vector3(tabMuscle.TValue[tid], tabMuscle.TValue[tid + 1], tabMuscle.TValue[tid + 2]);

            int qid = gid * 4;
            Quaternion rootQ = new Quaternion(tabMuscle.QValue[qid], tabMuscle.QValue[qid + 1], tabMuscle.QValue[qid + 2], tabMuscle.QValue[qid + 3] == 0 ? 1 : tabMuscle.QValue[qid + 3]);

            if (rootSelected == 0)
            {
                rootT = Handles.PositionHandle(rootT, target.transform.rotation);
                WriteTValue(tid, rootT.x);
                WriteTValue(tid + 1, rootT.y);
                WriteTValue(tid + 2, rootT.z);
            }
            else if (rootSelected == 1)
            {
                rootQ = Handles.RotationHandle(rootQ, rootT);
                WriteQValue(qid, rootQ.x);
                WriteQValue(qid + 1, rootQ.y);
                WriteQValue(qid + 2, rootQ.z);
                WriteQValue(qid + 3, rootQ.w);
            }
        }

        public void DrawMuscleHandle()
        {
            if (avatar == null)
                return;

            BoneInfo info = boneInfos[selected];
            if (info == null)
                return;

            Transform t = info.bone;

            if (getAxisLength == null)
                return;
            float axisLength = (float)getAxisLength.Invoke(avatar, new object[] { info.boneId });

            if (getPreRotation == null)
                return;
            Quaternion quaternion = (Quaternion)getPreRotation.Invoke(avatar, new object[] { info.boneId });

            if (getPostRotation == null)
                return;
            Quaternion quaternion2 = (Quaternion)getPostRotation.Invoke(avatar, new object[] { info.boneId });
            quaternion = t.parent.rotation * quaternion;
            quaternion2 = t.rotation * quaternion2;
            Color b = new Color(1f, 1f, 1f, 0.5f);

            if (getZYRoll == null)
                return;
            Quaternion zYRoll = (Quaternion)getZYRoll.Invoke(avatar, new object[] {
                info.boneId,
                Vector3.zero
            });

            if (getLimitSign == null)
                return;
            Vector3 limitSign = (Vector3)getLimitSign.Invoke(avatar, new object[] { info.boneId });
            Vector3 vector = quaternion2 * Vector3.right;
            Vector3 p = t.position + vector * axisLength;
            Handles.color = Color.white;
            Handles.DrawLine(t.position, p);
            if (info.muscleXId != -1)
            {
                if (getZYPostQ == null)
                    return;
                Quaternion zYPostQ = (Quaternion)getZYPostQ.Invoke(avatar, new object[] {
                    info.boneId,
                    t.parent.rotation,
                    t.rotation
                });

                float num4 = muscleMinMaxValues[info.muscleXId].x;
                float num5 = muscleMinMaxValues[info.muscleXId].y;
                vector = quaternion2 * Vector3.right;
                Vector3 vector2 = zYPostQ * Vector3.forward;
                Vector3 vector3 = t.position + vector * axisLength * 0.75f;
                vector = quaternion2 * Vector3.right * limitSign.x;
                Quaternion rotation = Quaternion.AngleAxis(num4, vector);
                vector2 = rotation * vector2;
                Handles.color = Handles.xAxisColor * b;
                Handles.DrawSolidArc(vector3, vector, vector2, num5 - num4, axisLength * 0.25f);
                vector2 = quaternion2 * Vector3.forward;
                Handles.color = Handles.centerColor;
                Handles.DrawLine(vector3, vector3 + vector2 * axisLength * 0.25f);
            }
            if (info.muscleYId != -1)
            {
                float num6 = muscleMinMaxValues[info.muscleYId].x;
                float num7 = muscleMinMaxValues[info.muscleYId].y;
                vector = quaternion * Vector3.up * limitSign.y;
                Vector3 vector2 = quaternion * zYRoll * Vector3.right;
                Quaternion rotation2 = Quaternion.AngleAxis(num6, vector);
                vector2 = rotation2 * vector2;
                Handles.color = Handles.yAxisColor * b;
                Handles.DrawSolidArc(t.position, vector, vector2, num7 - num6, axisLength * 0.25f);
            }
            if (info.muscleZId != -1)
            {
                float num8 = muscleMinMaxValues[info.muscleZId].x;
                float num9 = muscleMinMaxValues[info.muscleZId].y;
                vector = quaternion * Vector3.forward * limitSign.z;
                Vector3 vector2 = quaternion * zYRoll * Vector3.right;
                Quaternion rotation3 = Quaternion.AngleAxis(num8, vector);
                vector2 = rotation3 * vector2;
                Handles.color = Handles.zAxisColor * b;
                Handles.DrawSolidArc(t.position, vector, vector2, num9 - num8, axisLength * 0.25f);
            }
        }

        private void WriteTValue(int i, float value)
        {
            if (value == tabMuscle.TValue[i])
                return;
            tabMuscle.TValue[i] = value;
            tabMuscle.QToggle[i] = true;
            tabMuscle.WritePropertyValue(PSMuscleDefine.TProperty[i], value, true);
            // Resample animation immediately
            tabMuscle.ResampleAnimation();
        }

        private void WriteQValue(int i, float value)
        {
            if (value == tabMuscle.QValue[i])
                return;
            tabMuscle.QValue[i] = value;
            tabMuscle.QToggle[i] = true;
            tabMuscle.WritePropertyValue(PSMuscleDefine.QProperty[i], value, true);
            // Resample animation immediately
            tabMuscle.ResampleAnimation();
        }

        void MuscleGUI(int id)
        {
            BoneInfo info = boneInfos[selected];

            if (info.muscleXId != -1)
                DrawSlider(info.muscleXId, Handles.xAxisColor);
            if (info.muscleYId != -1)
                DrawSlider(info.muscleYId, Handles.yAxisColor);
            if (info.muscleZId != -1)
                DrawSlider(info.muscleZId, Handles.zAxisColor);
        }

        private void DrawSlider(int i, Color color)
        {
            string name = tabMuscle.muscleName[i];
            float muscleValue = tabMuscle.muscleValue[i];

            Color tmp = GUI.color;
            GUI.color = color;
            GUILayout.Label(string.Format("{0}: {1}", name, muscleValue), GUILayout.Width(180));
            GUI.color = tmp;

            float val = GUILayout.HorizontalSlider(muscleValue, -1, 1, GUILayout.Height(20));

            if (val == muscleValue)
                return;

            tabMuscle.muscleToggle[i] = true;
            tabMuscle.muscleValue[i] = val;
            tabMuscle.WriteMuscleValue(i, val);
            // Resample animation immediately
            tabMuscle.ResampleAnimation();
        }
    }
}