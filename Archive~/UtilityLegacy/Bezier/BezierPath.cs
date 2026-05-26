using System;
using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework.Utility.Bezier {
    public class BezierPath : MonoBehaviour {
        public List<CurveInfo> pathPoints = new();

        [Range(1, 20)]
        public int resolution = 10;

        private readonly List<TransformInfo> m_Result = new();

        private void Start() {
            InvokeRepeating(nameof(ComputePath), 0f, 0.2f);
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) ComputePath();

            if (m_Result.Count <= 0) return;

            var temp = m_Result[0].position;
            foreach (var pathPointInfo in m_Result) {
                Gizmos.color = Color.white;
                if (!pathPointInfo.position.Equals(temp)) {
                    Gizmos.DrawLine(temp, pathPointInfo.position);
                    temp = pathPointInfo.position;
                }

                Gizmos.DrawWireSphere(pathPointInfo.position, 0.05f);
                Gizmos.color = Color.green;
                //Debug.LogInfo(pathPointInfo.rotation.eulerAngles);
                //transform.InverseTransformPoint()
                Gizmos.DrawLine(pathPointInfo.position, pathPointInfo.position);
            }

            //Gizmos.DrawWireSphere(GetPointOnPath(progress), 0.5f);
        }

        private void OnValidate() {
            foreach (var info in pathPoints)
                if (info.handle == null)
                    info.curveType = CurveType.Linear;
        }

        public bool IsPointOnPath(Vector3 point, float allowedAngularError) {
            var closest = GetClosestPointOnPath(point);

            var delta = point - m_Result[closest].position;
            var angleWithNext = 0f;
            var angleWithLast = 0f;

            if (closest < m_Result.Count - 1) angleWithNext = Vector3.Angle(delta, m_Result[closest + 1].position - m_Result[closest].position);

            if (closest > 0) angleWithLast = Vector3.Angle(delta, m_Result[closest - 1].position - m_Result[closest].position);


            //first
            if (closest == 0) return angleWithNext < allowedAngularError;

            //last
            if (closest == m_Result.Count - 1) return angleWithLast < allowedAngularError;

            //rest
            return angleWithLast < allowedAngularError || angleWithNext < allowedAngularError;
        }

        // public int GetNextPathPointID(Vector3 currentPosition, int id) {
        //     if (m_Result.Count == 0) {
        //         return -1;
        //     }
        //
        //     if (m_Result.Count == 1) {
        //         return 0;
        //     }
        //
        //     var closest = GetClosestPointOnPath(currentPosition);
        //
        //     var delta = currentPosition - m_Result[closest].position;
        //
        //     //TODO: maybe change 0.1 to another value fuck!!!!
        //     var distance = currentPosition - m_Result[id].position;
        //     if (distance.magnitude < 0.2f && closest < m_Result.Count - 1) {
        //         Debug.LogInfo("Reached");
        //         return id + 1;
        //     }
        //
        //     var angleWithNext = 0f;
        //     var angleWithLast = 0f;
        //
        //     if (closest < m_Result.Count - 1) {
        //         angleWithNext = Vector3.Angle(delta, m_Result[closest + 1].position - m_Result[closest].position);
        //     }
        //
        //     if (closest > 0) {
        //         angleWithLast = Vector3.Angle(delta, m_Result[closest - 1].position - m_Result[closest].position);
        //     }
        //
        //     //first
        //     if (closest == 0) {
        //         return angleWithNext > 90f ? 0 : 1;
        //     }
        //
        //     //last
        //     if (closest == m_Result.Count - 1) {
        //         return closest;
        //     }
        //
        //     //rest
        //     if (angleWithNext < angleWithLast) {
        //         return closest + 1;
        //     }
        //
        //     if (angleWithNext < angleWithLast) {
        //         return closest;
        //     }
        //
        //     return closest;
        // }

        public List<TransformInfo> GetPathPoints() {
            return m_Result;
        }

        /// <summary>
        ///     Get position of certain progress on the path
        /// </summary>
        /// <param name="t"> global progress of whole path clamp from 0 to 1 </param>
        /// <returns></returns>
        public Vector3 GetPointOnPath(float t) {
            t = Mathf.Clamp01(t);

            var length = GetLocalProgress(t, out var id); //length from 0 to t

            var point = pathPoints[id];
            var next = pathPoints[id + 1];

            switch (point.curveType) {
                case CurveType.Linear:
                    return BezierHelper.GetLinearPoint(point.start.position, next.start.position, length);
                case CurveType.Quadratic:
                    return BezierHelper.GetQuadraticPoint(point.start.position, pathPoints[id + 1].start.position,
                        point.handle.position, length);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     calculate pathPoints[from].start to pathPoint[to].start
        /// </summary>
        /// <param name="from"> inclusive </param>
        /// <param name="to"> exclusive </param>
        /// <returns></returns>
        private float GetLengthOnPath(int from, int to) {
            if (from < 0 || to > pathPoints.Count - 1 || to - from <= 0) return 0f;

            if (pathPoints.Count <= 1) return 0f;

            var sum = 0f;
            for (var i = from; i < to; i++) {
                var delta = 0f;

                switch (pathPoints[i].curveType) {
                    case CurveType.Linear:
                        delta = (pathPoints[i + 1].start.position - pathPoints[i].start.position).magnitude;
                        break;
                    case CurveType.Quadratic:
                        delta = BezierHelper.GetQuadraticLength(pathPoints[i].start.position,
                            pathPoints[i + 1].start.position,
                            pathPoints[i].handle.position, resolution);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                sum += delta;
            }

            return sum;
        }

        private void ComputePath() {
            m_Result.Clear();
            if (pathPoints.Count == 0) return;
            if (pathPoints[0].start==null) return;
            if (pathPoints[0].handle==null) return;

            //start point
            var first = TransformInfo.CreateInstance();
            first.position = pathPoints[0].start.position;
            first.rotation = pathPoints[0].start.rotation;
            first.scale = pathPoints[0].start.localScale;
            m_Result.Add(first);

            for (var i = 0; i < pathPoints.Count - 1; i++) {
                var start = pathPoints[i];
                var end = pathPoints[i + 1];
                switch (start.curveType) {
                    case CurveType.Linear:
                        var item = TransformInfo.CreateInstance();
                        item.position = end.start.position;
                        item.rotation = end.start.rotation;
                        item.scale = end.start.localScale;
                        m_Result.Add(item);
                        break;
                    case CurveType.Quadratic:
                        //add all the point to the list
                        var items = BezierHelper.GetQuadraticPoints(start.start, end.start, start.handle, resolution);
                        //Debug.LogInfo(items);
                        m_Result.AddRange(items);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     get current t(progress) from a global t and return the id of the path point where global t is at;
        /// </summary>
        /// <param name="global"> global t </param>
        /// <param name="id"> id of path point </param>
        /// <returns> from 0 to 1 </returns>
        private float GetLocalProgress(float global, out int id) {
            id = -1;
            if (pathPoints.Count <= 1) return 0f;

            var length = global * GetLengthOnPath(0, pathPoints.Count - 1);

            var sum = 0f;
            for (var i = 0; i < pathPoints.Count - 1; i++) {
                var delta = 0f;

                if (pathPoints[i].handle == null)
                    delta = (pathPoints[i + 1].start.position - pathPoints[i].start.position).magnitude;
                else
                    delta = BezierHelper.GetQuadraticLength(pathPoints[i].start.position,
                        pathPoints[i + 1].start.position,
                        pathPoints[i].handle.position, resolution);

                if (delta + sum > length || Mathf.Approximately(delta + sum, length)) {
                    id = i;
                    return (length - sum) / delta;
                }

                sum += delta;
            }

            return 0f;
        }

        private int GetClosestPointOnPath(Vector3 position) {
            var closest = 0;
            for (var i = 0; i < m_Result.Count; i++) {
                var current = (m_Result[i].position - position).magnitude;
                if (current < (m_Result[closest].position - position).magnitude) closest = i;
            }

            return closest;
        }

        [Serializable]
        public class CurveInfo {
            public CurveType curveType;
            public Transform start;
            public Transform handle;
        }
    }
}