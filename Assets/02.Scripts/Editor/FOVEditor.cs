using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        //EnemyFOV 클래스를 참조한다. 
        EnemyFOV fov = (EnemyFOV)target;
        //원주 위의 시작점으 좌표를 계산 ( 주어진 각도의 1/2) 
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        //원 색상 설정
        Handles.color = Color.white;
        //외곽선만 표현 하는 원반을 그림 
        Handles.DrawSolidArc(fov.transform.position,    // 원점 좌표
            Vector3.up                                  // 노멀 벡터
            , fromAnglePos                              // 부채꼴의 시작 좌표
            , fov.viewAngle                             // 부채꼴의 각도
            , fov.viewRange);                           // 부채꼴의 반지름
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f)
            , fov.viewAngle.ToString());

    }
}
