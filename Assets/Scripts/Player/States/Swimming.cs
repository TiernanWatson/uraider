﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimming : PlayerStateBase<Swimming>
{
    private bool isEntering = false;
    private bool isTreading = false;

    public override void OnEnter(PlayerController player)
    {
        player.Anim.SetBool("isSwimming", true);
        isEntering = true;
        player.camController.PivotOnTarget();
    }

    public override void OnExit(PlayerController player)
    {
        player.Anim.SetBool("isSwimming", false);
        isEntering = false;
        player.camController.PivotOnPivot();
    }

    public override void Update(PlayerController player)
    {
        if (isEntering)
        {
            if (player.Velocity.y < 0f)
                player.ApplyGravity(-14);
            else
                isEntering = false;

            return;
        }

        if (!isTreading)
        {
            if (Input.GetKey(KeyCode.Space))
                SwimUp(player);
            else if (Input.GetKey(KeyCode.LeftShift))
                SwimDown(player);
            else
                player.MoveFree(player.swimSpeed);

            player.RotateToVelocity(4f);

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position + (Vector3.up * 0.5f), Vector3.down, out hit, 0.5f))
            {
                if (hit.transform.gameObject.CompareTag("Water"))
                {
                    isTreading = true;
                    player.Anim.SetBool("isTreading", true);
                    player.transform.position = hit.point + (1.48f * Vector3.down);
                }
            }
        }
        else
        {
            player.MoveGrounded(player.treadSpeed, false);
            player.RotateToVelocityGround(4f);
        }
    }

    private void SwimUp(PlayerController player)
    {

    }

    private void SwimDown(PlayerController player)
    {

    }
}