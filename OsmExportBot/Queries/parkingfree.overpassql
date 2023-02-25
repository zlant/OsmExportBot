[out:xml][timeout:25];
(
  nwr["amenity"="parking"]["fee"!="yes"]["access"!="private"]["access"!="customers"]["access"!="no"]({{bbox}});
  
  node["amenity"="parking_entrance"]["fee"!="yes"]["access"!="private"]["access"!="customers"]["access"!="no"]({{bbox}});
  
  way["parking:condition:left"="free"]({{bbox}});
  way["parking:condition:both"="free"]({{bbox}});
  way["parking:condition:right"="free"]({{bbox}});
);
out body;
>;
out skel qt;