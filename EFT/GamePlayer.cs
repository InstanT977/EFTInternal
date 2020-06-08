using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeFromGod
{
    public class GamePlayer
    {
        private Vector3 result;
        public Player Player { get; }
        public float Distance { get; private set; }
        public Vector3 Veloctiy { get; private set; }

		public Vector3 NextBone { get; private set; }
        public float Fov { get; private set; }

		public GamePlayer(Player player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			this.Player = player;
			this.Distance = (float)Math.Floor(Vector3.Distance(player.Transform.position, CGameWorld.MainCamera.transform.position));
			this.Veloctiy = player.Velocity;
            this.NextBone = getBonePos(player);
            this.Fov = CalcInFov();
		}
        private Vector3 getBonePos(Player p)
        {
            if (GetBonePosByID(p, intbone(bone.Head)) != Vector3.zero)
                return this.GetBonePosByID(p, intbone(bone.Head));
            else if (GetBonePosByID(p, intbone(bone.Neck)) != Vector3.zero)
                return this.GetBonePosByID(p, intbone(bone.Neck));
            else if (GetBonePosByID(p, intbone(bone.Chest)) != Vector3.zero)
                return this.GetBonePosByID(p, intbone(bone.Chest));
            else if (GetBonePosByID(p, intbone(bone.Stomach)) != Vector3.zero)
                return this.GetBonePosByID(p, intbone(bone.Stomach));
            else
                return Vector3.zero;
        }
        private float CalcInFov()
        {
            Vector3 position = Camera.main.transform.position;
            Vector3 forward = Camera.main.transform.forward;
            Vector3 normalized = (this.NextBone - position).normalized;
            return Mathf.Acos(Mathf.Clamp(Vector3.Dot(forward, normalized), -1f, 1f)) * 57.29578f;
        }
        private Vector3 GetBonePosByID(Player p, int id)
        {
            try
            {
                result = SkeletonBonePos(p.PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>().SkeletonRootJoint, id);
            }
            catch (Exception)
            {
                result = Vector3.zero;
            }
            return result;
        }

        private enum bone
        {
            Head,
            Neck,
            Chest,
            Stomach
        }

        private int intbone(bone num)
        {
            switch (num)
            {
                case bone.Neck:
                    return 132;

                case bone.Chest:
                    return 36;

                case bone.Stomach:
                    return 29;

                default:
                    return 133;
            }
        }

        private Vector3 SkeletonBonePos(Diz.Skinning.Skeleton sko, int id)
        {
            return sko.Bones.ElementAt(id).Value.position;
        }
    }

}
