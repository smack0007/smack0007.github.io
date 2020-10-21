---
Title: ID3DXEffect::OnLostDevice()
Date: 2012-07-20
Tags: .net, direct3D9, directx
---

I fixed a bug yesterday in Snowball related to a lost graphics device. I noticed that when I would use CTRL + ALT + DELETE, my apps were crashing. Turned out it was due to not recovering properly from a lost device.

The root of the problem was that I needed to call the ID3DXEffect::OnLostDevice() method when the device was being lost. This allows the effect to recover from the lost device. I've made the Effect class in Snowball now do this automatically.
