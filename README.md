Spektr/Lightning
================

*Lightning* is a specialized line shader/renderer, which draws electric bolts
between given two points.

![screenshot](http://36.media.tumblr.com/242092a5ed60bcbfec38dc33fec99dd1/tumblr_o022797Dls1qio469o2_r1_400.png)
![gif](http://49.media.tumblr.com/d18e5e57a2397b2897a6470881da66ee/tumblr_o022797Dls1qio469o1_400.gif)

*Lightning* is part of the *Spektr* effect suite. Please see the [GitHub
repositories][spektr] for further information about the suite.

[spektr]: https://github.com/search?q=spektr+user%3Akeijiro&type=Repositories

System Requirements
-------------------

Unity 5.1 or later versions.

*Lightning* only draws line segments of electric bolts, therefore it’s strongly
recommended to use with a [HDR bloom image effect][bloom] to add glow around the
line segments.

The *Lightning* shader only requires Shader Model 3.0 and might work on mobile
GPUs. However, it's not very practical to use with previous generation GPUs,
because it’s difficult to use without a HDR render target and linear rendering.

[bloom]: https://github.com/keijiro/KinoBloom

License
-------

Copyright (C) 2015 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
