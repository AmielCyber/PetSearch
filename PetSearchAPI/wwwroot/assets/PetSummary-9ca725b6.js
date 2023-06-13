import{j as e,T as i,a4 as o,a5 as c,a7 as u,b as N,P as j,M}from"./index-fc9c881b.js";import{G as s}from"./useToken-a2b8ef55.js";function P(t){return e.jsx(i,{variant:"h2",children:t.name})}var v={},R=c;Object.defineProperty(v,"__esModule",{value:!0});var b=v.default=void 0,k=R(o()),g=e,L=(0,k.default)([(0,g.jsx)("path",{d:"m16.17 19-2-2H6l3-4 2.25 3 .82-1.1L5 7.83V19zM7.83 5 19 16.17V5z",opacity:".3"},"0"),(0,g.jsx)("path",{d:"M19 5v11.17l2 2V5c0-1.1-.9-2-2-2H5.83l2 2H19zM2.81 2.81 1.39 4.22 3 5.83V19c0 1.1.9 2 2 2h13.17l1.61 1.61 1.41-1.41L2.81 2.81zM5 19V7.83l7.07 7.07-.82 1.1L9 13l-3 4h8.17l2 2H5z"},"1")],"HideImageTwoTone");b=v.default=L;var x={},O=c;Object.defineProperty(x,"__esModule",{value:!0});var D=x.default=void 0,A=O(o()),H=e,q=(0,A.default)((0,H.jsx)("path",{d:"M2 12c0 5.52 4.48 10 10 10s10-4.48 10-10S17.52 2 12 2 2 6.48 2 12zm18 0c0 4.42-3.58 8-8 8s-8-3.58-8-8 3.58-8 8-8 8 3.58 8 8zM8 12l4-4 1.41 1.41L11.83 11H16v2h-4.17l1.59 1.59L12 16l-4-4z"}),"ArrowCircleLeftOutlined");D=x.default=q;var m={},B=c;Object.defineProperty(m,"__esModule",{value:!0});var p=m.default=void 0,T=B(o()),w=e,E=(0,T.default)((0,w.jsx)("path",{d:"M22 12c0-5.52-4.48-10-10-10S2 6.48 2 12s4.48 10 10 10 10-4.48 10-10zM4 12c0-4.42 3.58-8 8-8s8 3.58 8 8-3.58 8-8 8-8-3.58-8-8zm12 0-4 4-1.41-1.41L12.17 13H8v-2h4.17l-1.59-1.59L12 8l4 4z"}),"ArrowCircleRightOutlined");p=m.default=E;const V={opacity:"0.7","&:hover":{opacity:"0.9"}};function _(t){return e.jsx(u,{"aria-label":"next-image",color:"primary",onClick:t.onClickNavigation,sx:V,children:t.children})}var h={},G=c;Object.defineProperty(h,"__esModule",{value:!0});var C=h.default=void 0,F=G(o()),J=e,K=(0,F.default)((0,J.jsx)("path",{d:"M12 2C6.47 2 2 6.47 2 12s4.47 10 10 10 10-4.47 10-10S17.53 2 12 2z"}),"Circle");C=h.default=K;var f={},Q=c;Object.defineProperty(f,"__esModule",{value:!0});var S=f.default=void 0,U=Q(o()),W=e,X=(0,U.default)((0,W.jsx)("path",{d:"M12 2C6.47 2 2 6.47 2 12s4.47 10 10 10 10-4.47 10-10S17.53 2 12 2zm0 18c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"}),"CircleOutlined");S=f.default=X;const Y="_navDots_6qhvl_1",Z={navDots:Y};function ee(t,a,r){const l=Array(t);for(let n=0;n<l.length;n++)n===a?l[n]=e.jsx("li",{children:e.jsx(u,{"aria-label":`Select image ${n+1}`,disabled:!0,children:e.jsx(C,{fontSize:"medium",color:"primary"})})},n):l[n]=e.jsx("li",{children:e.jsx(u,{"aria-label":`Select image ${n+1}`,onClick:()=>r(n),sx:te,children:e.jsx(S,{fontSize:"medium",color:"primary"})})},n);return l}const te={"&:hover":{backgroundColor:"rgb(189,189,189,0.4)"}};function ae(t){return e.jsx("div",{className:Z.navDots,children:e.jsx("menu",{children:ee(t.totalNavDots,t.currentIndex,t.onSelectDotNav)})})}const ne="_imageContainer_g8rbm_1",ie="_blankImage_g8rbm_18",re="_imgNavButtons_g8rbm_25",d={imageContainer:ne,blankImage:ie,imgNavButtons:re};function le(t){const[a,r]=N.useState(0);if(t.photos.length<1)return e.jsx(j,{elevation:4,children:e.jsx("div",{className:d.blankImage,children:e.jsx(b,{fontSize:"large"})})});const l=a>0,n=a<t.photos.length-1,y=()=>{r(l?a-1:t.photos.length-1)},$=()=>{r(n?a+1:0)},z=I=>{r(I)};return e.jsx(j,{elevation:4,children:e.jsxs("div",{className:d.imageContainer,children:[e.jsx("img",{src:t.photos[a].full,alt:t.name,sizes:"500px"}),t.photos.length>1&&e.jsxs(e.Fragment,{children:[e.jsxs("div",{className:d.imgNavButtons,children:[e.jsx(_,{onClickNavigation:y,children:e.jsx(D,{fontSize:"large"})}),e.jsx(_,{onClickNavigation:$,children:e.jsx(p,{fontSize:"large"})})]}),e.jsx(ae,{totalNavDots:t.photos.length,currentIndex:a,onSelectDotNav:z})]})]})})}function se(t){return e.jsxs(i,{variant:"body1",component:"section",children:[e.jsx(i,{variant:"subtitle1",children:e.jsx("b",{children:"Age"})}),t.petData.age,e.jsx(i,{variant:"subtitle1",children:e.jsx("b",{children:"Size"})}),t.petData.size,e.jsx(i,{variant:"subtitle1",children:e.jsx("b",{children:"Gender"})}),t.petData.gender,e.jsx(i,{variant:"subtitle1",children:e.jsx("b",{children:"Status"})}),t.petData.status]})}function oe(t){if(!t)return"No description provided.";const a=document.createElement("div");return a.innerHTML=t,a.textContent}function ce(t){const a=oe(t.description);return e.jsxs("section",{children:[e.jsx(i,{variant:"h4",children:"Description"}),e.jsx(i,{variant:"body1",children:a}),e.jsx(M,{href:t.url,children:"More at PetFinder.com."})]})}function ve(t){return e.jsxs(s,{container:!0,justifyContent:"center",columnSpacing:4,rowSpacing:3,alignItems:"center",children:[e.jsx(s,{item:!0,xs:12,textAlign:"center",mt:2,children:e.jsx(P,{name:t.petData.name})}),e.jsx(s,{item:!0,children:e.jsx(le,{name:t.petData.name,photos:t.petData.photos})}),e.jsx(s,{item:!0,textAlign:"center",children:e.jsx(se,{petData:t.petData})}),e.jsx(s,{item:!0,textAlign:"center",xs:12,children:e.jsx(ce,{description:t.petData.description,url:t.petData.url})})]})}export{ve as default};
