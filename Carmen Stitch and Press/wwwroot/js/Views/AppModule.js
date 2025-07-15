"use strict"

const App = {
    resources: {},
    createModule: function (namespace, module, dependencies) {
        let nsparts = namespace.split(".");
        let parent = App;

        //we want to be able to include or exlude the root namespace so we strip
        //if it it's in the namespace
        if (nsparts[0] === "App") {
            nsparts = nsparts.slice(1);
        }

        function f() {
            return module.apply(this, dependencies);
        }

        f.prototype = module.prototype;

        let innerModule = new f();

        //loop through the parts and create nested namespace if needed
        for (let i = 0, namespaceLength = nsparts.length; i < namespaceLength; i++) {
            var partName = nsparts[i];
            //check if the current parent already has the namespace declared
            //if it is not, then create it
            if (typeof parent[partName] === "undefined") {
                parent[partName] = (i === namespaceLength - 1) ? innerModule : {};
            }
            //get a reference to the deepest element in the hierarchy so far
            parent = parent[partName];
        }
        return parent;
    }
}