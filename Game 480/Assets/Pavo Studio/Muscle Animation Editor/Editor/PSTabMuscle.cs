using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PavoStudio.MAE
{
    [System.Serializable]
    class PSTabMuscle : PSTab
    {
        public bool[] muscleToggle;
        public string[] muscleName;
        public float[] muscleValue;
        public float[] muscleMasterValue;
        public float[] muscleFingerValue;
        public bool[] muscleBodyGroupToggle;
        public EditorCurveBinding[] curveBindings;
        public string[] propertyMuscleName;
        public int muscleCount;

        public int curSelectTQGroup;
        public int curSelectGroup;
        public int curSelectMasterGroup;
        public bool mirror;

        public float[] TValue;
        public bool[] TToggle;

        public float[] QValue;
        public bool[] QToggle;

        public bool showRootGUI = true;
        public bool showMuscleGroupGUI = true;
        public bool showMuscleGUI = true;
        public bool showBatchToolGUI = true;
        public bool showEulerAngle = true;

        private bool resample;
        private List<EditorCurveBinding> bindingList = new List<EditorCurveBinding>();

        private bool valueChanged;
        private Dictionary<int, float> changedValues = new Dictionary<int, float>();
        private bool propertyChanged;
        private Dictionary<string, KeyValuePair<float, bool>> changedProperties = new Dictionary<string, KeyValuePair<float, bool>>();

        private BatchData[] muscleBatches;
        private BatchData[] TQBatches;
        private string[] operators = { "Add", "Subtract", "Multiply", "Divide" };

        [System.Serializable]
        public class BatchData
        {
            public string[] propertyNames;
            public int selectedProperty;
            public int selectedOperator;
            public float value;
        }

        public PSTabMuscle()
        {
            //Init ();
        }

        public void Init()
        {
            int len = PSMuscleDefine.muscle.Length;
            this.muscleBodyGroupToggle = new bool[len];
            for (int i = 0; i < len; i++)
            {
                this.muscleBodyGroupToggle[i] = false;
            }
            this.muscleName = PSMuscleDefine.GetMuscleName();

            this.propertyMuscleName = PSMuscleDefine.GetPropertyMuscleName();

            this.muscleCount = HumanTrait.MuscleCount;
            this.muscleToggle = new bool[this.muscleCount];
            this.muscleValue = new float[this.muscleCount];

            len = PSMuscleDefine.masterMuscle.Length;
            this.muscleMasterValue = new float[len];

            len = PSMuscleDefine.fingerMuscle.Length;
            this.muscleFingerValue = new float[len];

            len = PSMuscleDefine.TProperty.Length;
            TValue = new float[len];
            TToggle = new bool[len];

            len = PSMuscleDefine.QProperty.Length;
            QValue = new float[len];
            QToggle = new bool[len];

            Undo.undoRedoPerformed += UndoCallback;
        }

        private void InitTQBatches()
        {
            int groupLen = PSMuscleDefine.TQGroup.Length;
            TQBatches = new BatchData[groupLen];
            for (int i = 0; i < groupLen; i++)
            {
                string[] names = new string[7];

                int tid = i * 3;
                names[0] = PSMuscleDefine.TProperty[tid];
                names[1] = PSMuscleDefine.TProperty[tid + 1];
                names[2] = PSMuscleDefine.TProperty[tid + 2];

                int qid = i * 4;
                names[3] = PSMuscleDefine.QProperty[qid];
                names[4] = PSMuscleDefine.QProperty[qid + 1];
                names[5] = PSMuscleDefine.QProperty[qid + 2];
                names[6] = PSMuscleDefine.QProperty[qid + 3];

                TQBatches[i] = new BatchData()
                {
                    propertyNames = names
                };
            }
        }

        private void InitMuscleBatches()
        {
            int groupLen = muscleBodyGroupToggle.Length;
            muscleBatches = new BatchData[groupLen];
            for (int i = 0; i < groupLen; i++)
            {
                int[] array = PSMuscleDefine.muscle[i];
                string[] names = new string[array.Length];
                for (int j = 0, len = array.Length; j < len; j++)
                {
                    names[j] = this.curveBindings[array[j]].propertyName;
                }

                muscleBatches[i] = new BatchData()
                {
                    propertyNames = names
                };
            }
        }

        void UndoCallback()
        {
            resample = true;
            OnUpdateValue();
        }

        /***********************************************
	     *  override 
	     ***********************************************/

        public override void OnTargetChange()
        {
            if (target == null)
            {
                PSLogger.Log("Target is null");
                return;
            }

            EditorCurveBinding[] bindings = AnimationUtility.GetAnimatableBindings(target, target);
            bindingList.Clear();

            for (int i = 0; i < this.propertyMuscleName.Length; i++)
            {
                for (int j = 0; j < bindings.Length; j++)
                {
                    EditorCurveBinding binding = bindings[j];
                    if (binding.type.Equals(typeof(Animator)) && binding.propertyName == this.propertyMuscleName[i])
                    {
                        bindingList.Add(binding);
                        break;
                    }
                }
            }
            curveBindings = bindingList.ToArray();
        }

        public override void OnUpdateValue()
        {
            if (target == null || clip == null)
            {
                PSLogger.Log("Target or clip is null");
                return;
            }

            for (int i = 0; i < muscleCount; i++)
            {
                muscleToggle[i] = false;
                muscleValue[i] = 0f;
            }

            for (int i = 0, len = TValue.Length; i < len; i++)
            {
                TValue[i] = 0;
                TToggle[i] = false;
            }

            for (int i = 0, len = QValue.Length; i < len; i++)
            {
                QValue[i] = 0;
                QToggle[i] = false;
            }

            foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(clip))
            {
                if (!binding.type.Equals(typeof(Animator)))
                    continue;
                //Debug.Log(binding.propertyName);

                bool flag = false;
                string propertyName = binding.propertyName;

                for (int i = 0, len = TValue.Length; i < len; i++)
                {
                    if (propertyName == PSMuscleDefine.TProperty[i])
                    {
                        //Do not use AnimationUtility.GetFloatValue as the value is different from the curve value
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                        TValue[i] = curve.Evaluate(time);
                        TToggle[i] = true;
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    continue;

                for (int i = 0, len = QValue.Length; i < len; i++)
                {
                    if (propertyName == PSMuscleDefine.QProperty[i])
                    {
                        //Do not use AnimationUtility.GetFloatValue as the value is different from the curve value
                        AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                        QValue[i] = curve.Evaluate(time);
                        QToggle[i] = true;
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    continue;

                for (int i = 0, len = this.propertyMuscleName.Length; i < len; i++)
                {
                    if (binding.propertyName == this.propertyMuscleName[i])
                    {
                        AnimationUtility.GetFloatValue(target, binding, out muscleValue[i]);
                        muscleToggle[i] = true;
                        break;
                    }
                }
            }
        }

        public override void OnTabGUI()
        {
            this.TQGUI();
            EditorGUILayout.Space();
            this.MuscleGroupGUI();
            EditorGUILayout.Space();
            this.MuscleGUI();
            EditorGUILayout.Space();
        }

        /***********************************************
	 *  GUI
	 ***********************************************/

        protected void TQGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            showRootGUI = EditorGUILayout.Foldout(showRootGUI, "T/Q Group");

            if (GUILayout.Button("Reset", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                for (int i = 0, len = PSMuscleDefine.TProperty.Length; i < len; i++)
                {
                    TValue[i] = 0;
                    TToggle[i] = false;
                    WritePropertyValue(PSMuscleDefine.TProperty[i], 0, false);
                }

                for (int i = 0, len = PSMuscleDefine.QProperty.Length; i < len; i++)
                {
                    QValue[i] = 0;
                    QToggle[i] = false;
                    WritePropertyValue(PSMuscleDefine.QProperty[i], 0, false);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (!showRootGUI)
                return;

            int groupLen = PSMuscleDefine.TQGroup.Length;
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < groupLen; i++)
            {
                if (GUILayout.Toggle(curSelectTQGroup == i, PSMuscleDefine.TQGroup[i], EditorStyles.toolbarButton))
                {
                    curSelectTQGroup = i;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            for (int i = 0; i < groupLen; i++)
            {
                if (curSelectTQGroup != i)
                    continue;

                RootTGUI(i);
                RootQGUI(i);

                if (TQBatches == null || TQBatches.Length == 0)
                    InitTQBatches();

                EditorGUILayout.Space();

                BatchToolGUI(TQBatches[i]);
            }
        }

        private void RootTGUI(int gid)
        {
            int startIndex = gid * 3;
            bool groupToggle = TToggle[startIndex] || TToggle[startIndex + 1] || TToggle[startIndex + 2];
            bool toggle = EditorGUILayout.BeginToggleGroup("T", groupToggle);
            bool toggleChange = toggle != groupToggle;

            EditorGUI.indentLevel++;
            for (int i = 0; i < 3; i++)
            {
                int tid = startIndex + i;
                float oldValue = TValue[tid];
                if (toggleChange)
                {
                    TToggle[tid] = toggle;
                    WritePropertyValue(PSMuscleDefine.TProperty[tid], oldValue, toggle);
                }

                EditorGUILayout.BeginHorizontal();

                float value = EditorGUILayout.FloatField(PSMuscleDefine.TLabel[i], oldValue);
                if (oldValue != value)
                {
                    WritePropertyValue(PSMuscleDefine.TProperty[tid], value, true);

                    TValue[tid] = value;
                    TToggle[tid] = true;
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.EndToggleGroup();

        }

        private void RootQGUI(int gid)
        {
            int startIndex = gid * 4;
            int idx = startIndex;
            int idy = startIndex + 1;
            int idz = startIndex + 2;
            int idw = startIndex + 3;

            bool groupToggle = QToggle[idx] || QToggle[idy] || QToggle[idz] || QToggle[idw];

            bool toggle = EditorGUILayout.BeginToggleGroup("Q", groupToggle);

            if (toggle != groupToggle)
            {
                QToggle[idx] = toggle;
                QToggle[idy] = toggle;
                QToggle[idz] = toggle;
                QToggle[idw] = toggle;

                // init values
                if (QValue[idw] == 0)
                {
                    Quaternion quat = Quaternion.identity;
                    QValue[idx] = quat.x;
                    QValue[idy] = quat.y;
                    QValue[idz] = quat.z;
                    QValue[idw] = quat.w;
                }

                WritePropertyValue(PSMuscleDefine.QProperty[idx], QValue[idx], toggle);
                WritePropertyValue(PSMuscleDefine.QProperty[idy], QValue[idy], toggle);
                WritePropertyValue(PSMuscleDefine.QProperty[idz], QValue[idz], toggle);
                WritePropertyValue(PSMuscleDefine.QProperty[idw], QValue[idw], toggle);
            }

            EditorGUI.indentLevel++;

            Quaternion q = new Quaternion(QValue[idx], QValue[idy], QValue[idz], QValue[idw]);
            if (showEulerAngle)
            {
                float x = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[0], q.eulerAngles.x);
                float y = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[1], q.eulerAngles.y);
                float z = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[2], q.eulerAngles.z);

                if (x != q.eulerAngles.x || y != q.eulerAngles.y || z != q.eulerAngles.z)
                {
                    q.eulerAngles = new Vector3(x, y, z);
                    QValue[idx] = q.x;
                    QValue[idy] = q.y;
                    QValue[idz] = q.z;
                    QValue[idw] = q.w;
                    WritePropertyValue(PSMuscleDefine.QProperty[idx], q.x, QToggle[idx]);
                    WritePropertyValue(PSMuscleDefine.QProperty[idy], q.y, QToggle[idy]);
                    WritePropertyValue(PSMuscleDefine.QProperty[idz], q.z, QToggle[idz]);
                    WritePropertyValue(PSMuscleDefine.QProperty[idw], q.w, QToggle[idw]);
                }
            }
            else
            {
                float x = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[3], q.x);
                float y = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[4], q.y);
                float z = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[5], q.z);
                float w = EditorGUILayout.FloatField(PSMuscleDefine.QLabel[6], q.w);

                if (x != q.x)
                {
                    QValue[idx] = x;
                    WritePropertyValue(PSMuscleDefine.QProperty[idx], x, QToggle[idx]);
                }

                if (y != q.y)
                {
                    QValue[idy] = y;
                    WritePropertyValue(PSMuscleDefine.QProperty[idy], y, QToggle[idy]);
                }

                if (z != q.z)
                {
                    QValue[idz] = z;
                    WritePropertyValue(PSMuscleDefine.QProperty[idz], z, QToggle[idz]);
                }

                if (w != q.w)
                {
                    QValue[idw] = w;
                    WritePropertyValue(PSMuscleDefine.QProperty[idw], w, QToggle[idw]);
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(GUIContent.none);
            if (GUILayout.Button(showEulerAngle ? "Quaternion" : "Euler Angle", EditorStyles.miniButton, GUILayout.Width(100)))
            {
                showEulerAngle = !showEulerAngle;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndToggleGroup();
        }

        protected void MuscleGroupGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            showMuscleGroupGUI = EditorGUILayout.Foldout(showMuscleGroupGUI, "Muscle Group");

            if (GUILayout.Button("Reset Sliders", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                for (int k = 0, len = PSMuscleDefine.masterMuscle.Length; k < len; k++)
                {
                    muscleMasterValue[k] = TabSlider(PSMuscleDefine.muscleTypeGroup[k], 0);
                }
                for (int k = 0, len = PSMuscleDefine.fingerMuscle.Length; k < len; k++)
                {
                    muscleFingerValue[k] = TabSlider(PSMuscleDefine.muscleFingerGroup[k], 0);
                }
            }
            //EditorGUILayout.LabelField ("Muscle Group");
            EditorGUILayout.EndHorizontal();

            if (!showMuscleGroupGUI)
                return;

            EditorGUILayout.BeginHorizontal();
            for (int i = 0, len = PSMuscleDefine.muscleTabGroup.Length; i < len; i++)
            {
                if (GUILayout.Toggle(curSelectMasterGroup == i, PSMuscleDefine.muscleTabGroup[i], EditorStyles.toolbarButton))
                {
                    curSelectMasterGroup = i;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("This window affects multiple muscles’ value at the same time. Note however, that the changes made here will affect the ‘Muscle Body Group’ as well. Changes made in Muscle Body Group will not affect the Muscle Group properties.", MessageType.None);

            if (curSelectMasterGroup == 0)
            {
                for (int k = 0, len = PSMuscleDefine.masterMuscle.Length; k < len; k++)
                {
                    float oldValue = muscleMasterValue[k];
                    float value = TabSlider(PSMuscleDefine.muscleTypeGroup[k], oldValue);

                    if (oldValue == value)
                        continue;

                    muscleMasterValue[k] = value;
                    int[] array = PSMuscleDefine.masterMuscle[k];
                    for (int l = 0, len2 = array.Length; l < len2; l++)
                    {
                        int index = array[l];

                        if (index != -1)
                        {
                            muscleValue[index] = value;
                            muscleToggle[index] = true;
                            WriteMuscleValue(index, muscleValue[index]);
                        }
                    }
                }
            }
            else if (curSelectMasterGroup == 1)
            {
                for (int k = 0, len = PSMuscleDefine.fingerMuscle.Length; k < len; k++)
                {
                    float oldValue = muscleFingerValue[k];
                    float value = TabSlider(PSMuscleDefine.muscleFingerGroup[k], oldValue);
                    if (oldValue == value)
                        continue;

                    muscleFingerValue[k] = value;
                    int[] array = PSMuscleDefine.fingerMuscle[k];
                    for (int l = 0, len2 = array.Length; l < len2; l++)
                    {
                        int index = array[l];

                        if (index != -1)
                        {
                            muscleValue[index] = value;
                            muscleToggle[index] = true;
                            WriteMuscleValue(index, muscleValue[index]);
                        }
                    }
                }
            }
        }

        protected void MuscleGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            showMuscleGUI = EditorGUILayout.Foldout(showMuscleGUI, "Muscle Body Group");

            mirror = GUILayout.Toggle(mirror, "Mirror", EditorStyles.toolbarButton, GUILayout.Width(80));

            if (GUILayout.Button("Add All", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                for (int j = 0; j < muscleValue.Length; j++)
                {
                    muscleToggle[j] = true;
                }
                WriteAllMuscleValue();
            }

            if (GUILayout.Button("Remove All", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                for (int j = 0; j < muscleValue.Length; j++)
                {
                    muscleValue[j] = 0f;
                    muscleToggle[j] = false;
                }
                WriteAllMuscleValue();
            }
            EditorGUILayout.EndHorizontal();

            if (!showMuscleGUI)
                return;

            int groupLen = muscleBodyGroupToggle.Length;
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < groupLen; i++)
            {
                if (GUILayout.Toggle(curSelectGroup == i, PSMuscleDefine.muscleBodyGroup[i], EditorStyles.toolbarButton))
                {
                    curSelectGroup = i;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            for (int i = 0; i < groupLen; i++)
            {
                if (curSelectGroup != i)
                    continue;

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(PSMuscleDefine.muscleBodyGroup[i], GUILayout.MinWidth(100));

                if (GUILayout.Button("Add All", EditorStyles.miniButtonLeft, GUILayout.Width(70)))
                {
                    for (int j = 0, len = PSMuscleDefine.muscle[i].Length; j < len; j++)
                    {
                        WriteToggleValue(i, j, true);
                    }
                }

                if (GUILayout.Button("Remove All", EditorStyles.miniButtonRight, GUILayout.Width(70)))
                {
                    for (int j = 0, len = PSMuscleDefine.muscle[i].Length; j < len; j++)
                    {
                        WriteToggleValue(i, j, false);
                    }
                }

                GUILayout.EndHorizontal();
                EditorGUILayout.Space();

                int[] array = PSMuscleDefine.muscle[i];
                for (int j = 0, len = array.Length; j < len; j++)
                {
                    int index = array[j];

                    GUILayout.BeginHorizontal();

                    bool toggle = EditorGUILayout.ToggleLeft(GUIContent.none, muscleToggle[index], GUILayout.Width(10));

                    if (toggle != muscleToggle[index])
                    {
                        WriteToggleValue(i, j, toggle);
                    }

                    EditorGUI.BeginDisabledGroup(!muscleToggle[index]);

                    float value = TabSlider(this.muscleName[index], muscleValue[index]);

                    if (muscleValue[index] != value)
                    {
                        muscleValue[index] = value;

                        if (mirror)
                            WriteMirrorMuscleValue(i, j);

                        WriteMuscleValue(index, value);
                    }

                    EditorGUI.EndDisabledGroup();
                    GUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Change values");

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Copy values to the mirror side", EditorStyles.miniButton, GUILayout.Width(180)))
                {
                    for (int j = 0, len = PSMuscleDefine.muscle[i].Length; j < len; j++)
                    {
                        WriteMirrorMuscleValue(i, j);
                    }
                }

                // MCV
                if (GUILayout.Button("Flip values Vertically", EditorStyles.miniButton, GUILayout.Width(180)))
                {
                    for (int j = 0, len = PSMuscleDefine.muscle[i].Length; j < len; j++)
                    {
                        WriteFlipVerticalMuscleValue(i, j);
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();

                if (muscleBatches == null || muscleBatches.Length == 0)
                    InitMuscleBatches();

                BatchToolGUI(muscleBatches[i]);
            }

        }

        protected void BatchToolGUI(BatchData batchData)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Change values for all key frames");

            EditorGUILayout.BeginHorizontal();
            batchData.selectedProperty = EditorGUILayout.Popup(batchData.selectedProperty, batchData.propertyNames);

            batchData.selectedOperator = EditorGUILayout.Popup(batchData.selectedOperator, operators, GUILayout.Width(70));
            batchData.value = EditorGUILayout.FloatField(batchData.value, GUILayout.Width(70));
            EditorGUILayout.Space();
            if (GUILayout.Button("Apply", EditorStyles.miniButton, GUILayout.Width(70)))
            {
                BatchApply(batchData.propertyNames[batchData.selectedProperty], batchData.selectedOperator, batchData.value);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
        }

        private void BatchApply(string propertyName, int selectedOperator, float value)
        {
            EditorCurveBinding binding = new EditorCurveBinding();
            bool found = false;
            EditorCurveBinding[] bindings = AnimationUtility.GetAnimatableBindings(target, target);

            for (int j = 0; j < bindings.Length; j++)
            {
                binding = bindings[j];
                if (binding.type.Equals(typeof(Animator)) && binding.propertyName == propertyName)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return;

            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
            if (curve == null || curve.keys == null || curve.keys.Length == 0)
                return;

            Keyframe[] keyframes = curve.keys;
            for (int i = 0, len = curve.keys.Length; i < len; i++)
            {
                switch (selectedOperator)
                {
                    case 0:
                        keyframes[i].value += value;
                        break;
                    case 1:
                        keyframes[i].value -= value;
                        break;
                    case 2:
                        keyframes[i].value *= value;
                        break;
                    case 3:
                        if (value == 0)
                            break;
                        keyframes[i].value /= value;
                        break;
                }
            }

            curve.keys = keyframes;
            AnimationUtility.SetEditorCurve(clip, binding, curve);
            resample = true;
        }

        /***********************************************
	     *  Write Values
	     ***********************************************/

        public void WriteToggleValue(int i, int j, bool toggle)
        {
            int index = PSMuscleDefine.muscle[i][j];

            muscleToggle[index] = toggle;
            int i2 = PSMuscleDefine.mirrorMuscle[i];

            if (mirror && i2 != i)
            {
                int mirrorIndex = PSMuscleDefine.muscle[i2][j];
                muscleToggle[mirrorIndex] = toggle;
                WriteMuscleValue(mirrorIndex, muscleValue[mirrorIndex]);
            }

            WriteMuscleValue(index, muscleValue[index]);
        }

        public void WriteMirrorMuscleValue(int i, int j)
        {
            int index = PSMuscleDefine.muscle[i][j];

            int i2 = PSMuscleDefine.mirrorMuscle[i];
            if (i2 == i)
                return;

            int mirrorIndex = PSMuscleDefine.muscle[i2][j];
            muscleValue[mirrorIndex] = muscleValue[index];
            muscleToggle[mirrorIndex] = muscleToggle[index];
            WriteMuscleValue(mirrorIndex, muscleValue[mirrorIndex]);
        }

        //MCV - FlipVertical
        // This should switch the Left & Right sides to invert the pose
        public void WriteFlipVerticalMuscleValue(int i, int j)
        {
            int index = PSMuscleDefine.muscle[i][j];
            int i2 = PSMuscleDefine.mirrorMuscle[i];
            if (i2 == i)
                return;

            int mirrorIndex = PSMuscleDefine.muscle[i2][j];
            float tempMuscleValue = muscleValue[mirrorIndex];         // Save the current mirrored location value.
            bool tempMuscleToggle = muscleToggle[mirrorIndex];
            muscleValue[mirrorIndex] = muscleValue[index];
            muscleToggle[mirrorIndex] = muscleToggle[index];
            WriteMuscleValue(mirrorIndex, muscleValue[mirrorIndex]);

            // Copy the temp to the original side and save.
            muscleValue[index] = tempMuscleValue;
            muscleToggle[index] = tempMuscleToggle;
            WriteMuscleValue(index, muscleValue[index]);
        }

        public void WritePropertyValue(string propertyName, float value, bool enable)
        {
            if (clip == null)
                return;

            changedProperties[propertyName] = new KeyValuePair<float, bool>(value, enable);
            propertyChanged = true;
        }

        public void WriteMuscleValue(int index, float value)
        {
            if (curveBindings == null)
            {
                OnTargetChange();
            }

            if (clip == null || curveBindings == null)
            {
                PSLogger.Error("Clip or curveBindings is null");
                return;
            }

            if (index >= curveBindings.Length)
            {
                PSLogger.Error("Array index is out of range");
                return;
            }

            changedValues[index] = value;
            valueChanged = true;
        }

        public void WriteAllMuscleValue()
        {
            if (curveBindings == null)
            {
                OnTargetChange();
            }

            if (clip == null || curveBindings == null)
            {
                PSLogger.Error("Clip or curveBindings is null");
                return;
            }

            if (this.propertyMuscleName.Length > curveBindings.Length)
            {
                PSLogger.Error("CurveBindings length and propertyMuscleName length are not equal");
                return;
            }

            for (int i = 0, len = this.propertyMuscleName.Length; i < len; i++)
            {
                changedValues[i] = muscleValue[i];
            }

            valueChanged = true;
        }

        public void UpdateAnimationClip()
        {
            if (valueChanged)
            {
                valueChanged = false;
                Undo.RecordObject(clip, clip.name);

                foreach (var pair in changedValues)
                {
                    if (muscleToggle[pair.Key])
                        this.SetEditorCurve(curveBindings[pair.Key], pair.Value);
                    else
                        AnimationUtility.SetEditorCurve(clip, curveBindings[pair.Key], null);
                }
                changedValues.Clear();
                resample = true;
            }

            if (propertyChanged)
            {
                propertyChanged = false;
                Undo.RecordObject(clip, clip.name);

                foreach (var pair in changedProperties)
                {
                    EditorCurveBinding binding = EditorCurveBinding.FloatCurve("", typeof(Animator), pair.Key);
                    Undo.RecordObject(clip, clip.name);

                    if (pair.Value.Value)
                        this.SetEditorCurve(binding, pair.Value.Key);
                    else
                        AnimationUtility.SetEditorCurve(clip, binding, null);
                }
                changedProperties.Clear();
                resample = true;
            }
        }

        private void SetEditorCurve(EditorCurveBinding binding, float value)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

            if (curve == null)
                curve = new AnimationCurve();

            bool found = false;
            Keyframe[] keys = curve.keys;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].time == time)
                {
                    keys[i].value = value;
                    found = true;
                    break;
                }
            }

            if (found)
                curve.keys = keys;
            else
                curve.AddKey(new Keyframe(time, value));

            AnimationUtility.SetEditorCurve(clip, binding, curve);
        }

        public void ResampleAnimation()
        {
            resample = true;
        }

        public void UpdateAnimation()
        {
            if (!resample)
                return;

            resample = false;

            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(target, clip, time);
                AnimationMode.EndSampling();
            }

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private static float TabSlider(string label, float val)
        {
            if (label == null)
                val = EditorGUILayout.Slider(GUIContent.none, val, -1f, 1f);
            else
                val = EditorGUILayout.Slider(label, val, -1f, 1f);
            return val;
        }
    }
}
