using UnityEngine;
using System.Collections.Generic;

public class CupheadOverwolrdAnimatorManager : MonoBehaviour {
  private Animator animator;

  public enum Animation {
    MoveUp,
    MoveDown,
    MoveDiagonalUp,
    MoveDiagonalDown,
    MoveFront,
    StandUp,
    StandDown,
    StandDiagonalUp,
    StandDiagonalDown,
    StandFront,
    HappyJump,
  }
  public Animation lastMovementAnimation;

  public Dictionary<Animation, string> animations = new Dictionary<Animation, string>();
  public Dictionary<(int, int), Animation> moveAnimations = new Dictionary<(int, int), Animation>();
  public Dictionary<Animation, Animation> standAnimations = new Dictionary<Animation, Animation>();

  private void Awake() {
    animator = GetComponent<Animator>();
    animations[Animation.MoveUp] = "MoveUp";
    animations[Animation.MoveDown] = "MoveDown";
    animations[Animation.MoveDiagonalUp] = "MoveDiagonalUp";
    animations[Animation.MoveDiagonalDown] = "MoveDiagonalDown";
    animations[Animation.MoveFront] = "MoveFront";
    animations[Animation.StandUp] = "StandUp";
    animations[Animation.StandDown] = "StandDown";
    animations[Animation.StandDiagonalUp] = "StandDiagonalUp";
    animations[Animation.StandDiagonalDown] = "StandDiagonalDown";
    animations[Animation.StandFront] = "StandFront";
    animations[Animation.HappyJump] = "HappyJump";

    moveAnimations[(0,1)] = Animation.MoveUp;
    moveAnimations[(1,1)] = Animation.MoveDiagonalUp;
    moveAnimations[(-1,1)] = Animation.MoveDiagonalUp;
    moveAnimations[(1,0)] = Animation.MoveFront;
    moveAnimations[(-1,0)] = Animation.MoveFront;
    moveAnimations[(1,-1)] = Animation.MoveDiagonalDown;
    moveAnimations[(-1,-1)] = Animation.MoveDiagonalDown;
    moveAnimations[(0,-1)] = Animation.MoveDown;

    standAnimations[Animation.MoveUp] = Animation.StandUp;
    standAnimations[Animation.MoveDiagonalUp] = Animation.StandDiagonalUp;
    standAnimations[Animation.MoveFront] = Animation.StandFront;
    standAnimations[Animation.MoveDiagonalDown] = Animation.StandDiagonalDown;
    standAnimations[Animation.MoveDown] = Animation.StandDown;
    moveAnimations[(0,0)] = Animation.StandDown;
  }

  public void PlayAnimation(Animation name) {
    animator.Play(animations[name]);
  }
}
