import React, { useEffect, useRef, useState } from 'react';
import useInterval from 'use-interval';
import * as d3 from 'd3';

// Credit https://wattenberger.com/blog/react-and-d3
export default function Svg() {
  const generateDataset = () => (
    Array(10).fill(0).map(() => ([
      Math.random() * 80 + 10,
      Math.random() * 35 + 10,
    ]))
  );

  const [dataset, setDataset] = useState(generateDataset());
  const ref = useRef();

  useEffect(() => {
    // https://www.d3indepth.com/v4/enterexit/
    renderCircles(ref, dataset);
  }, [dataset]);// Render  when component loaded and dataset changes

  // https://overreacted.io/making-setinterval-declarative-with-react-hooks/
  useInterval(() => {
    const newDataset = generateDataset();
    setDataset(newDataset);
  }, 5000);

  return <svg viewBox="0 0 100 50" ref={ref} />;
};

function renderCircles(ref: React.MutableRefObject<undefined>, dataset: number[][]) {
  const joining = d3.select(ref.current)
    .selectAll<SVGCircleElement, number[]>('circle')
    .data(dataset);
  console.log('joining', joining);
  console.log('joining before remove', joining.nodes.length);

  // Remove data point which does not exist in a new set
  const exitSelection = joining.exit();
  exitSelection.remove();
  console.log('exit', exitSelection);
  console.log('joining after remove', joining.nodes.length);

  const enterSelection = joining.enter();
  console.log('enter', enterSelection);

  const mergedSelection = enterSelection
    .append('circle')
    .merge(joining)
    // update attribute to new and existing data points
    .attr('cx', (d) => d[0])
    .attr('cy', (d) => d[1])
    .attr('r', 3);

  console.log('merged', mergedSelection);

}
