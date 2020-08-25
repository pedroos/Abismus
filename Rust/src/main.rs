fn main() {
    //let fh = FnHolder {f: mirror};
    //let fhf = fh.f;
    //println!("{}", fhf(3));
    
    //let fht = FnHolderT {f: mirror};
    //let fhtf = fht.f;
    //println!("{}", fhtf(4));
    
    //let mut vec = Vec::new();
    //vec.push(FnHolderT {f: square});
    //vec.push(FnHolderT {f: mirror}); // vec assumes type of first element
    
    // try a trait instead of struct
    // println!("TEST:");

    let node = node::Node {};
    // println!("{}", fixed_node.o());

    // let trt1 = Node::FnTraitIO::<i32, i32>::mirror;

    println!("{}", node::FnTraitO::<i32>::random(&node));
    println!("{}", node::FnTraitIO::mirror(&node, 6));



    // "boxed" or traits objects (dynamic dispatch)
    // let mut vec2 = Vec::<Node>::new();
    // vec2.push(node);
    // vec2.push(random_fn);

    // println!("EXECUTE:");
    
    // for f in &vec2 {
    //     // let val = f.f();
    //     // println!("{}", val);
    // }

    // For this to be productive, we need to match over traits and execute a common function.
    // This should be like:
    // if (is traitO) cv = traitO.f();
    // if (is traitIO) cv = traitIO.f(cv)

    // let fn_o = Node2::Node2 {};
    // println!("{}", Node2::FnTraitFixed::f(&fn_o));
    // println!("{}", Node2::FnTraitSquare::f(&fn_o, 6));
    let fn_o = node2::NodeO {};
    println!("{}", node2::FnTraitFixed::<i32>::f(&fn_o));
    let fn_io = node2::NodeIO {};
    println!("{}", node2::FnTraitSquare::<i32, i32>::f(&fn_io, 4));

    // let mut vec3 = Vec::<Node2::Node2>::new();
    // vec3.push(fn_o);
}

// fn mirror(a: i32) -> i32 {
//     return a;
// }

// fn square(a: f32) -> f32 {
//     return a * a;
// }

// struct FnHolder {
//     // type: fn pointer `fn(i32) -> i32`
//     f: fn(i32) -> i32
// }

// struct FnHolderT<T> {
//     f: fn(T) -> T
// }

mod node {
    use rand::Rng;

    pub trait FnTraitO<TO> {
        fn fixed(&self) -> TO;
        fn random(&self) -> TO;
    }

    pub trait FnTraitIO<TI, TO> {
        fn mirror(&self, in1: TI) -> TO;
    }

    // trait FnTraitIOO<TI, TO1, TO2> { // see tuples later
    //     fn IOO(&self, in1: TI) -> (TO, TO);
    // }

    pub trait FnTraitIIO<TI1, TI2, TO> {
        fn mult(&self, in1: TI1, in2: TI2) -> TO;
    }

    pub struct Node {

    }

    impl FnTraitO<i32> for Node {
        fn fixed(&self) -> i32 {
            return 5;
        }
        fn random(&self) -> i32 {
            let mut rng = rand::thread_rng();
            return rng.gen_range(1, 10);
        }
    }

    impl FnTraitO<f32> for Node {
        fn fixed(&self) -> f32 {
            return 5.5;
        }
        fn random(&self) -> f32 {
            let mut rng = rand::thread_rng();
            return rng.gen_range(1., 10.);
        }
    }

    impl FnTraitIO<i32, i32> for Node {
        fn mirror(&self, in1 : i32) -> i32 {
            return in1;
        }
    }
}

mod node2 {
    // pub struct Node {}

    pub struct NodeO {}

    pub struct NodeIO {}

    // pub trait FnTraitO<TO> {
    //     fn f(&self) -> TO;
    // }

    pub trait FnTraitFixed<TO> {
        fn f(&self) -> TO;
    }

    // pub trait FnTraitIO<TI, TO> {
    //     fn f(&self, i1: TI) -> TO;
    // }

    pub trait FnTraitSquare<TI, TO> {
         fn f(&self, i1: TI) -> TO;
    }

    impl FnTraitFixed<i32> for NodeO {
        fn f(&self) -> i32 {
            return 5;
        }
    }

    impl FnTraitSquare<i32, i32> for NodeIO {
        fn f(&self, i1: i32) -> i32 {
            return i1 * i1;
        }
    }
}

mod funcs1 {
    pub trait FnTrait {
        fn iiio<TI1, TI2, TI3, TO1>(&self, i1: TI1, i2: TI2, i3: TI3) -> TO1;
    }

    impl FnMirror for FnTrait {
        fn iiio<TI1, TI2, TI3, TO1>(&self, i1: TI1, _: TI2, _: TI3) -> TO1 {
            return i1;
        }
    }
}